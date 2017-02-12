using System;
using System.Threading;

namespace KafktaListener
{
    public class DebugKafkaListener
    {
        public DebugKafkaListener(KafkaSpout listener)
        {
            var saveListener = listener;
            //new Thread(() =>
            //{
            //    foreach (var msg in saveListener.MessageSpout.GetConsumingEnumerable())
            //    {
            //        Console.WriteLine($"Got Offset {msg.Offset} on {msg.TextKey} of {msg.Text}");
            //    }
            //}).Start();
            saveListener.WhenMessageReceived.Subscribe(
                msg=> Console.WriteLine($"Got Offset {msg.Offset} on {msg.TextKey} of {msg.Text}"),
                ex => Console.WriteLine($"Got Exception {ex.Message}"));
        }
    }
}