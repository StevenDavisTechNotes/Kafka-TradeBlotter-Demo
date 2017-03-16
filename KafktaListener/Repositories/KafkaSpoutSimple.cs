using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using log4net.Repository.Hierarchy;

namespace KafktaListener
{
    public class KafkaSpoutSimple
    {
        public static log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(KafkaSpoutSimple));
        public string Topic { get; }

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

        private string GetHostName()
        {
            var shortHostName = Dns.GetHostName();
            var fullHostName = Dns.GetHostEntry(shortHostName).HostName;
            return fullHostName;
        }

        public KafkaSpoutSimple(string topic)
        {
            Topic = topic;
            var thread = new Thread(MessagePumpLoop) { Name = $"KafkaSpout on {Topic}"};
            thread.Start();
        }

        private void MessagePumpLoop()
        {
            var options = new KafkaOptions(new Uri("http://"+Properties.Settings.Default.KafkaUrl))
            {
                Log = new ConsoleLog()
            };
            //var consumer = new Consumer(new ConsumerOptions(Topic, new BrokerRouter(options)) { Log = new ConsoleLog() });
            var router = new BrokerRouter(options);
            var consumer = new Consumer(new ConsumerOptions(Topic, router) {Log = new ConsoleLog()});

            //Consume returns a blocking IEnumerable (ie: never ending stream)
            foreach (var message in consumer.Consume())
            {
                Console.WriteLine("Response: P{0},O{1} : {2}",
                    message.Meta.PartitionId, message.Meta.Offset, message.Value);
                var convertedMsg = new KafkaEvent()
                {
                    Offset = message.Meta.Offset,
                    TextKey = message.Key == null ? "" : Encoding.UTF8.GetString(message.Key),
                    Text = Encoding.UTF8.GetString(message.Value)
                };
                MessageReceived?.Invoke(convertedMsg);
            }
        }
    }
}
