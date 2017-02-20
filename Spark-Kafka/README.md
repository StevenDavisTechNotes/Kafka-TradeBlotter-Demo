````
cd Scala-Docker
  docker build -t scala-docker .
cd ../Build-Docker
  docker build -t build-docker .
  docker run -it --name my-build-container build-docker bash
    sbt assembly
    exit
./copyToTest
  mkdir ../test
  cd ../test
  docker cp my-build-container:/src/target/scala-2.10/direct_kafka_word_count.jar .
  cp ../Build-Docker/log4j.properties .
  cp ../Build-Docker/listen.py .
  wget http://central.maven.org/maven2/org/apache/spark/spark-streaming-kafka-assembly_2.10/1.5.1/spark-streaming-kafka-assembly_2.10-1.5.1.jar
cd ../MyKafka
  docker build -t mykakfa .
cd ..
  docker-compose up
````