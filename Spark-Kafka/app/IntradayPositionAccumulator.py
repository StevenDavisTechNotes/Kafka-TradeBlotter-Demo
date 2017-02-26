#!/usr/bin/python
# use pyspark --jars /app/spark-streaming-kafka-assembly_2.10-1.5.1.jar
# spark-submit \
#     --jars /app/spark-streaming-kafka-assembly_2.10-1.5.1.jar \
#     --master spark://localhost:7077 \
#     IntradayPosition.py
import time
from pyspark.sql import SQLContext
from pyspark import SparkContext, SparkConf, AccumulatorParam
from pyspark.streaming import StreamingContext
from pyspark.streaming.kafka import KafkaUtils
import json
logger = sc._jvm.org.apache.log4j
logger.LogManager.getLogger("org"). setLevel( logger.Level.ERROR )
logger.LogManager.getLogger("akka").setLevel( logger.Level.ERROR )

ssc = StreamingContext(sc, 1) # 1 second window
dstrAllMessages = KafkaUtils\
  .createDirectStream(ssc, ["SodHoldings", "Execution"], \
    {"metadata.broker.list": "SparkTest:9092"})\
    .filter(lambda x: x is not None)\
    .map(lambda x: json.loads(x[1]))
dstrSodHoldings = dstrAllMessages\
  .filter(lambda x: x['Type'] == 'SodHoldings')\
  .flatMap(lambda x: [(row['Security'], row) for row in x['Data']])
dstrExecutions = dstrAllMessages\
  .filter(lambda x: x['Type'] == 'Execution')\
  .map(lambda row: (row['Data']['Security'], row['Data']))

# def getLastSodHoldingsAccumulator(sparkContext):
#     if 'LastSodHoldings' not in globals():
#         globals()['LastSodHoldings'] = sparkContext.accumulator({})
#     return globals()['LastSodHoldings']

securities = ['SEDOL1', 'SEDOL2', 'SEDOL3']

class NetExecutionsAccumulatorParam(AccumulatorParam):
    def zero(self, initialValue):
        global securities
        return {
            'TradingDay': 0, 
            'Data': dict([(x, {'DoneAmount':0, 'CostBasis':0}) for x in securities])}
    def addInPlace(self, v1, v2):
        if v1['TradingDay'] < v2['TradingDay']:
            return v2
        if v1['TradingDay'] > v2['TradingDay']:
            return v1
        global securities
        d1 = v1['Data']
        d2 = v2['Data']
        for security in securities:
            d1[security]['DoneAmount'] += d2[security]['DoneAmount']
            d1[security]['CostBasis'] += d2[security]['CostBasis']
        return v1

netExecutionAccumulatorsParam = NetExecutionsAccumulatorParam()
netExecutionAccumulators = sc.accumulator(netExecutionAccumulatorsParam.zero({}), netExecutionAccumulatorsParam)
def addExecutionToNetExecutions(executionPair):
    execution = executionPair[1]
    security = execution['Security']
    global netExecutionAccumulators
    global netExecutionAccumulatorsParam
    netExecution = netExecutionAccumulatorsParam.zero({})
    netExecution['TradingDay'] = execution['TradingDay']
    netExecution['Data'][security]['DoneAmount'] = execution['FillAmount']
    netExecution['Data'][security]['CostBasis'] = execution['FillAmount']*execution['FillPrice']
    netExecutionAccumulators += netExecution
    return (security, json.dumps(netExecution))

dstrNetExecutions = dstrExecutions.map(addExecutionToNetExecutions)
dstrJoined = dstrSodHoldings.join(dstrExecutions)
dstrNetExecutions.pprint()
# counts = lines.flatMap(lambda line: line.split(" ")) \
# 	.map(lambda word: (word, 1)) \
# 	.reduceByKey(lambda a, b: a+b)
# counts.pprint()

ssc.start()
time.sleep(2)
ssc.stop()
print "Got ", netExecutionAccumulators.value
# ssc.awaitTermination()
exit()

