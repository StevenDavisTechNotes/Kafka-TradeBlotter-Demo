````
cd MyKafka
  docker build -t mykafka .
  cd ..
cd MySpark
  docker build -t myspark .
  cd ..
cd Scala-Docker
  docker build -t scala-docker .
  cd ..
cd Build-MySparkApp-Docker
  docker build -t build-mysparkapp-docker .
  docker run --name my-build-container -it build-mysparkapp-docker sbt assembly
  docker cp my-build-container:/src/target/scala-2.10/direct_kafka_word_count.jar ../app
  docker rm my-build-container
  cd ..
cd app
  wget http://central.maven.org/maven2/org/apache/spark/spark-streaming-kafka-assembly_2.10/1.5.1/spark-streaming-kafka-assembly_2.10-1.5.1.jar
cd ..
  export MY_PUBLIC_IP="$(curl -4 http://l2.io/ip)"
  sudo sed -i "/ SparkTest$/d" /etc/hosts
  sudo sed -i "2i$MY_PUBLIC_IP SparkTest" /etc/hosts
  # docker-compose up
  docker-compose run spark
Verify Working Kafka
  docker exec -it $(docker-compose ps -q kafka) bash
    kafka-topics.sh --create --zookeeper $ZOOKEEPER --replication-factor 1 --partitions 2 --topic word-count
    kafka-topics.sh --list --zookeeper $ZOOKEEPER
    kafka-topics.sh --describe --zookeeper $ZOOKEEPER --topic word-count
    kafka-console-producer.sh --broker-list $KAFKA --topic word-count
      Hello, it's me
      I was wondering if after all these years you'd like to meet
      To go over everything
      They say that time's supposed to heal ya
      But I ain't done much healing
    kafka-console-consumer.sh --bootstrap-server $KAFKA --zookeeper=$ZOOKEEPER --topic word-count --from-beginning
    #kafka-topics.sh --delete --zookeeper $ZOOKEEPER --topic word-count
    exit
Verify Working Spark
  docker exec -it $(docker-compose ps -q spark) bash
    spark-submit \
      --master yarn-client \
      --driver-java-options \
      "-Dlog4j.configuration=file:///app/log4j.properties" \
      --class com.example.spark.DirectKafkaWordCount \
      app/direct_kafka_word_count.jar kafka:9092 word-count
    exit
Install Node.JS
  cd ~/Kafka-Blotter/Publishers
    sudo apt-get remove -y nodejs npm nodejs-legacy
    sudo apt install -y appstream/xenial-backports
    sudo appstreamcli refresh --force
    sudo apt-get update
    sudo apt-get install -y nodejs npm nodejs-legacy
    npm install
Run Demo
  docker exec -it $(docker-compose ps -q kafka) bash
  cd ~/Kafka-Blotter/Publishers
    node producers.js
  cd ~/Kafka-Blotter/Publishers
    node consumer.js
  docker exec -it $(docker-compose ps -q spark) bash
    clear && spark-submit \
      --jars /app/spark-streaming-kafka-assembly_2.10-1.5.1.jar \
      --master spark://localhost:7077 \
      IntradayPosition.py

  
````