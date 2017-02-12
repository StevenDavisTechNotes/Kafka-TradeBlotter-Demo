using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KafktaListener;
using TradingAnalytics.Services;

namespace TradingAnalytics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to close");
            var cts = new CancellationTokenSource();
            var exposuresObservable = ValuedRTPositionService.MakeExposuresObservable(cts.Token);
            DebugKafkaListener.PrintExposure(exposuresObservable);
            var strExpsouresObservable = ValuedRTPositionService.SerializeExposures(exposuresObservable);
            new KafkaSink("Exposures", strExpsouresObservable, cts);
            Console.ReadLine();
            cts.Cancel();
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            Environment.Exit(0);
        }
    }
}
