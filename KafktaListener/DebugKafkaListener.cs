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
                msg =>
                {
                    Console.WriteLine($"Got Offset {msg.Offset} on {msg.TextKey} of {msg.Text}");
                },
                ex => Console.WriteLine($"Got Exception {ex.Message}"));
        }

        public static void PrintExposure(IObservable<Exposure[]> observable)
        {
            observable.Subscribe(
                exposures =>
                {
                    if (exposures.Any())
                    {
                        Console.WriteLine(
                            $@"At {
                                exposures.Max(x => x.QuoteDate):hh:mm:ss} on Day {
                                exposures.Max(x => x.TradingDay)} got {
                                exposures.Sum(x => x.DoneAmount)} shares valued at {
                                exposures.Sum(x => x.DoneExposureUSD)} P/L= {
                                exposures.Sum(x => x.DoneIntradayPLUSD)}");
                    }
                    else
                    {
                        Console.WriteLine("Not exposures found");
                    }
                },
                ex => Console.WriteLine($"Got Exception {ex.Message}"));
        }
    }
}