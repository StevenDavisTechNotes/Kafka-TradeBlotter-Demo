#!/usr/bin/python
# use pyspark --jars /app/spark-streaming-kafka-assembly_2.10-1.5.1.jar
from pyspark.sql import SQLContext
from pyspark import SparkContext, SparkConf
from pyspark.streaming import StreamingContext
from pyspark.streaming.kafka import KafkaUtils
from uuid import uuid1

import json

#conf = SparkConf() \
#	.setAppName("Test") \
#	.setMaster("local") \
#	.set("spark.default.Parallelism",1) \
#	.set("spark.executor.memory","512m")
	
#sc = SparkContext(conf=conf)
sql = SQLContext(sc)
ssc = StreamingContext(sc, 1) # 1 second window

kafka_stream = KafkaUtils.createStream(ssc, \
                                       "SparkTest:2181", \
                                       "Quotes",
                                        {"pageviews":1})
lines = kafka_stream.map(lambda x: x[1])
counts = lines.flatMap(lambda line: line.split(" ")) \
	.map(lambda word: (word, 1)) \
	.reduceByKey(lambda a, b: a+b)
counts.pprint()

ssc.start()
ssc.awaitTermination()

