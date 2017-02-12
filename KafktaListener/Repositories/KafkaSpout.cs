using System;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using Kafka.Client.Cfg;
using Kafka.Client.Consumers;
using Kafka.Client.Helper;
using Kafka.Client.Messages;


namespace KafktaListener.Repositories
{
    public class KafkaSpout
    {
        public static log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(KafkaSpoutSimple));
        public string zTopic { get; }

        public class KafkaEvent
        {
            public long Offset { get; set; }
            public string TextKey { get; set; }
            public string Text { get; set; }
        }

        private event Action<KafkaEvent> MessageReceived;

        public IObservable<KafkaEvent> WhenMessageReceived
        {
            get
            {
                return Observable.FromEvent<KafkaEvent>(
                    h => this.MessageReceived += h,
                    h => this.MessageReceived -= h);
            }
        }

        public KafkaSpout(string topic, CancellationToken cancellationToken)
        {
            zTopic = topic;
            var thread = new Thread(()=>MessagePumpLoop(topic, cancellationToken)) { Name = $"KafkaSpout on {zTopic}" };
            thread.Start();
        }

        #region Implementation
        private void MessagePumpLoop(string topic, CancellationToken cancellationToken)
        {
            string TopicName = topic;
            string ClientId = $"KafkaSpout for {topic}";
            var urlZooKeeper = new Uri("http://" + Properties.Settings.Default.ZookeeperUrl);
            string Host = urlZooKeeper.Host;
            int PartitionId = 0;

            var brokerConfig = new BrokerConfiguration()
            {
                BrokerId = 0,
                Host = Host,
                Port = 9092,
            };

            var consumerConfig = new ConsumerConfiguration
            {
                GroupId = "1",
                Broker = brokerConfig,
                ZooKeeper = KafkaClientHelperUtils.ToZookeeperConfig(Properties.Settings.Default.ZookeeperUrl)
            };

            KafkaSimpleManagerConfiguration simpleConfig = new KafkaSimpleManagerConfiguration
            {
                Zookeeper = string.Format("{0}:2181", Host)
            };

            simpleConfig.Verify();

            //var producerConfiguration = new ProducerConfiguration(new List<BrokerConfiguration> { brokerConfig })
            //{
            //    ClientId = ClientId,
            //    ZooKeeper = consumerConfig.ZooKeeper
            //};

            //var kafkaProducer = new Producer(producerConfiguration);
            var correlationID = 0;

            using (KafkaSimpleManager<string, Message> kafkaSimpleManager = new KafkaSimpleManager<string, Message>(simpleConfig))
            {
                var allPartitions = kafkaSimpleManager.GetTopicPartitionsFromZK(TopicName);
                kafkaSimpleManager.RecreateSyncProducerPoolForMetadata();

                var topicMetadata = kafkaSimpleManager.RefreshMetadata(0, ClientId, correlationID++, TopicName, true);
                //var simpleProducerId = kafkaSimpleManager.InitializeProducerPoolForTopic(0, ClientId, correlationID, TopicName, true, new ProducerConfiguration(producerConfiguration), true);
                //var simpleProducer = kafkaSimpleManager.GetProducerOfPartition(TopicName, PartitionId, true);
                var consumer = kafkaSimpleManager.GetConsumer(TopicName, PartitionId);

                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    //var batch = "Wiadomosc testowa " + DateTime.UtcNow;
                    //var data = new ProducerData<string, Message>(TopicName, new Message(System.Text.Encoding.Default.GetBytes(batch)));
                    //simpleProducer.Send(data);  // Sends data here

                    for (var i = 0; i <= topicMetadata.PartitionsMetadata.Max(r => r.PartitionId); i++)
                    {
                        long earliest = 0;
                        long latest = 0;

                        kafkaSimpleManager.RefreshAndGetOffset(0, ClientId, correlationID, TopicName, PartitionId, true, out earliest, out latest);
                        var latestOffset = KafkaClientHelperUtils.GetValidStartReadOffset(KafkaOffsetType.Latest, earliest, latest, 100, 1);
                        var consumerData = consumer.FetchAndGetDetail(Host, TopicName, 0, PartitionId, latestOffset, int.MaxValue, 1000, 1000); // read data here

                        foreach (var message in consumerData.MessageAndOffsets.Select(x=>x.Message))
                        {
                            var convertedMsg = new KafkaEvent()
                            {
                                Offset = message.Offset,
                                TextKey = message.Key == null ? "" : Encoding.UTF8.GetString(message.Key),
                                Text = Encoding.UTF8.GetString(message.Payload)
                            };
                            MessageReceived?.Invoke(convertedMsg);
                        }
                    }
                }
            }
            Logger.Info($"{Thread.CurrentThread.Name} completed cancellation");
        }
#endregion
    }
}
