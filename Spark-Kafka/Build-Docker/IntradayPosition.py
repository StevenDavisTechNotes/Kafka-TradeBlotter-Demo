#!/usr/bin/python
# use pyspark  --master 'local[3]' --jars '/app/spark-streaming-kafka-assembly_2.10-1.5.1.jar'
# spark-submit \
#     --jars /app/spark-streaming-kafka-assembly_2.10-1.5.1.jar \
#     --master spark://localhost:7077 \
#     IntradayPosition.py
import time
import json
from pyspark.sql import SQLContext
from pyspark import SparkContext, SparkConf, AccumulatorParam
from pyspark.streaming import StreamingContext
from pyspark.streaming.kafka import KafkaUtils
from kafka import KafkaProducer

producer = KafkaProducer(bootstrap_servers='localhost:9092')

logger = sc._jvm.org.apache.log4j
logger.LogManager.getLogger("org"). setLevel(logger.Level.ERROR)
logger.LogManager.getLogger("akka").setLevel(logger.Level.ERROR)

ssc = StreamingContext(sc, 1)  # 1 second window
ssc.checkpoint('/app/checkpointDirectory')
dstrAllMessages = KafkaUtils\
    .createDirectStream(ssc, ["SodHoldings", "Execution"],
                        {"metadata.broker.list": "SparkTest:9092"})\
    .filter(lambda x: x is not None)\
    .map(lambda x: json.loads(x[1]))
dstrSodHoldings = dstrAllMessages\
    .filter(lambda x: x['Type'] == 'SodHoldings')\
    .flatMap(lambda x: [(row['Security'], row) for row in x['Data']])
dstrExecutions = dstrAllMessages\
    .filter(lambda x: x['Type'] == 'Execution')\
    .map(lambda row: row['Data'])\
    .map(lambda row: (row['Security'], {
        'Security': row['Security'],
        'TradingDay': row['TradingDay'],
        'FillPrice': row['FillPrice'],
        'FillAmount': row['FillAmount'],
        'CostBasis': row['FillPrice'] * row['FillPrice']
    }))

def AccumulateSodHoldings(v1, v2):
    if v1['TradingDay'] < v2['TradingDay']:
        return v2
    if v1['TradingDay'] > v2['TradingDay']:
        return v1
    return v1

lastSodHoldings = dstrSodHoldings.reduceByKey(AccumulateSodHoldings)

def AccumulateExecutions(v1, v2):
    if v1['TradingDay'] < v2['TradingDay']:
        return v2
    if v1['TradingDay'] > v2['TradingDay']:
        return v1
    v1['FillAmount'] += v2['FillAmount']
    v1['CostBasis'] += v2['CostBasis']
    v1['FillPrice'] = v1['CostBasis'] / v1['FillAmount']
    return v1

def InverseAccumulateExecutions(v1, v2):
    if v1['TradingDay'] < v2['TradingDay']:
        return v2
    if v1['TradingDay'] > v2['TradingDay']:
        return v1
    return v1

dstrNetExecution = dstrExecutions.reduceByKeyAndWindow(AccumulateExecutions, InverseAccumulateExecutions, 20, 1)

def LastSodHoldingsUpdater(new_values, last_value):
    n = len(new_values)
    if n == 0:
        return last_value
    new_last_value = new_values[n-1]
    if last_value is None:
        return new_last_value
    return new_last_value if new_last_value['TradingDay']>last_value['TradingDay'] else last_value

dstrSodHoldingsTicking = dstrSodHoldings\
    .updateStateByKey(LastSodHoldingsUpdater)

def CombineHoldingsAndExecutions(args):
    security, (sodHolding, netExecution) = args
    sodHolding['DoneAmount'] = sodHolding['SodAmount']
    if sodHolding['TradingDay'] == netExecution['TradingDay']:
        sodHolding['DoneAmount'] += netExecution['FillAmount']
        sodHolding['CostBasis'] += netExecution['CostBasis']
    return sodHolding

dstrJoined = dstrSodHoldingsTicking.join(dstrNetExecution)\
  .map(CombineHoldingsAndExecutions)

dstrJoined.pprint()

def WriteBackToKafka(message):
    records = message.collect()
    producer.send('RtPosition', str(records))
    producer.flush()

dstrJoined.foreachRDD(WriteBackToKafka)

ssc.start()
time.sleep(20)
ssc.stop()
# ssc.awaitTermination()
exit()
