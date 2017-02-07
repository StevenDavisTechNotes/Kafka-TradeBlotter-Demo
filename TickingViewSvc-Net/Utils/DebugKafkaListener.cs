using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using TickingViewSvc_Net.Repositories;

namespace TickingViewSvc_Net.Utils
{
    public class DebugKafkaListener
    {
        public DebugKafkaListener(KafkaSpout listener)
        {
            var saveListener = listener;
            new Thread(() =>
            {
                foreach (var msg in saveListener.MessageSpout.GetConsumingEnumerable())
                {
                    Console.WriteLine($"Got Offset {msg.Offset} on {msg.TextKey} of {msg.Text}");
                }
            }).Start();
        }
    }
}