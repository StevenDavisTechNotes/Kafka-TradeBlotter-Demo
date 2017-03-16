using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kafka.Client.Cfg;
using Kafka.Client.Consumers;
using Kafka.Client.Exceptions;
using Kafka.Client.Helper;
using Kafka.Client.Messages;
using Kafka.Client.Producers;

namespace KafktaListener
{
    public class KafkaSink
    {
        public static log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(KafkaSpoutSimple));
        public string Topic { get; }
        public KafkaSink(string topic, IObservable<string> observable, CancellationTokenSource cts)
        {
            Topic = topic;

            string TopicName = topic;
            string ClientId = $"KafkaSink for {Topic}";
            var urlKafka = new Uri("http://" + Properties.Settings.Default.KafkaUrl);
            int PartitionId = 0;

            var brokerConfig = new BrokerConfiguration()
            {
                BrokerId = 0,
                Host = urlKafka.Host,
                Port = urlKafka.Port,
            };

            var consumerConfig = new ConsumerConfiguration
            {
                GroupId = "1",
                Broker = brokerConfig,
                ZooKeeper = KafkaClientHelperUtils.ToZookeeperConfig(Properties.Settings.Default.ZookeeperUrl)
            };

            KafkaSimpleManagerConfiguration simpleConfig = new KafkaSimpleManagerConfiguration
            {
                Zookeeper = Properties.Settings.Default.ZookeeperUrl
            };

            simpleConfig.Verify();

            var producerConfiguration = new ProducerConfiguration(new List<BrokerConfiguration> { brokerConfig })
            {
                ClientId = ClientId,
                ZooKeeper = consumerConfig.ZooKeeper
            };

            var kafkaProducer = new Producer(producerConfiguration);
            var correlationID = 0;

            var kafkaSimpleManager = new KafkaSimpleManager<string, Message>(simpleConfig);
            var allPartitions = kafkaSimpleManager.GetTopicPartitionsFromZK(TopicName);
            kafkaSimpleManager.RecreateSyncProducerPoolForMetadata();

            var topicMetadata = kafkaSimpleManager.RefreshMetadata(0, ClientId, correlationID++, TopicName, true);
            var simpleProducerId = kafkaSimpleManager.InitializeProducerPoolForTopic(0, ClientId, correlationID, TopicName, true, new ProducerConfiguration(producerConfiguration), true);
            var simpleProducer = kafkaSimpleManager.GetProducerOfPartition(TopicName, PartitionId, true);

            observable.Subscribe(
                payload =>
                {
                    var data = new ProducerData<string, Message>(TopicName, new Message(Encoding.UTF8.GetBytes(payload)));
                    simpleProducer.Send(data);  // Sends data here
                },
                ex => Console.WriteLine($"{ClientId} received exception {ex.Message}"),
                cts.Token);
        }
    }
}
