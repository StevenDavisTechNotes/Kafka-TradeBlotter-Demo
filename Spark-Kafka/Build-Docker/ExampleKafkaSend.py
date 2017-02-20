# clear && spark-submit --jars spark-streaming-kafka-assembly_2.10-1.5.1.jar s.py

from pyspark import SparkConf, SparkContext
from operator import add
import sys
from pyspark.streaming import StreamingContext
from pyspark.streaming.kafka import KafkaUtils
import json
from kafka import SimpleProducer, KafkaClient
from kafka import KafkaProducer

producer = KafkaProducer(bootstrap_servers='SparkTest:9092')

def handler(message):
    records = message.collect()
    for record in records:
        producer.send('RtPositions', str(record))
        producer.flush()

def main():
    sc = SparkContext(appName="PythonStreamingDirectKafkaWordCount")
    ssc = StreamingContext(sc, 10)
    kvs = KafkaUtils.createDirectStream(ssc, ['Execution'], {"metadata.broker.list": 'SparkTest:9092'})
    kvs.foreachRDD(handler)
    ssc.start()
    ssc.awaitTermination()

if __name__ == "__main__":
    main()


# clear && pyspark  --master 'local[3]' --jars '/app/spark-streaming-kafka-assembly_2.10-1.5.1.jar'

from pyspark import SparkConf, SparkContext
from operator import add
import sys
from pyspark.streaming import StreamingContext
from pyspark.streaming.kafka import KafkaUtils
import json
from kafka import SimpleProducer, KafkaClient
from kafka import KafkaProducer

producer = KafkaProducer(bootstrap_servers='SparkTest:9092')

def handler(message):
    records = message.collect()
    for record in records:
        producer.send('RtPositions', str(record))
        producer.flush()

sc = SparkContext(appName="PythonStreamingDirectKafkaWordCount")
ssc = StreamingContext(sc, 10)
kvs = KafkaUtils.createDirectStream(ssc, ['Execution'], {"metadata.broker.list": 'SparkTest:9092'})
kvs.foreachRDD(handler)
ssc.start()
ssc.awaitTermination()


