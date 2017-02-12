using System;
using System.Linq;
using KafktaListener.Repositories;
using TickingViewSvc_Net.Models;

namespace KafktaListener
{
    public static class DebugKafkaListener
    {
        public static void PrintFromSpout(KafkaSpout listener)
        {
            listener.WhenMessageReceived.Subscribe(
                msg=> Console.WriteLine($"Got Offset {msg.Offset} on {msg.TextKey} of {msg.Text}"),
                ex => Console.WriteLine($"Got Exception {ex.Message}"));
        }

        public static void PrintExposure(IObservable<Exposure[]> observable)
        {
            observable.Subscribe(
                rtPositions =>
                {
                    Console.WriteLine(
                        $"At {rtPositions.Max(x => x.QuoteDate)} got {rtPositions.Sum(x => x.DoneAmount)} shares valued at {rtPositions.Sum(x => x.DoneExposureUSD)} P/L= {rtPositions.Sum(x => x.DoneIntradayPLUSD)}");
                },
                ex => Console.WriteLine($"Got Exception {ex.Message}"));
        }
    }
}