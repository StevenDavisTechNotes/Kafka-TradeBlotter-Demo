using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using KafktaListener;
using KafktaListener.Models;
using KafktaListener.Repositories;
using Newtonsoft.Json;
using TickingViewSvc_Net.Models;

namespace TradingAnalytics.Services
{
    public static class ValuedRTPositionService
    {
        public static IObservable<Exposure[]> MakeExposuresObservable(CancellationToken cancellationToken)
        {
            var lsnSodHoldings = new KafkaSpout("SodHoldings", cancellationToken);
            var obSodHoldings =
                lsnSodHoldings.WhenMessageReceived.Select(
                    kafkaEvent => JsonConvert.DeserializeObject<SodHolding[]>(kafkaEvent.Text)
                    );
            var lsnExecution = new KafkaSpout("Execution", cancellationToken);
            var obExecution =
                lsnExecution.WhenMessageReceived.Select(
                    kafkaEvent => JsonConvert.DeserializeObject<Execution>(kafkaEvent.Text)
                    );
            var obAggExecution = obExecution.Scan(
                seed: new Dictionary<string, Position>(),
                accumulator: (Dictionary<string, Position> prev, Execution execution) =>
                {
                    var prevTradingDay = prev.Values.FirstOrDefault()?.TradingDay;

                    Dictionary<string, Position> next;
                    if (prevTradingDay.HasValue && prevTradingDay.Value == execution.TradingDay)
                    {
                        next = new Dictionary<string, Position>(prev);
                    }
                    else
                    {
                        next = new Dictionary<string, Position>();
                    }
                    var newPosition = new Position()
                    {
                        Security = execution.Security,
                        Custodian = execution.Custodian,
                        SodAmount = 0m,
                        TargetAmount = 0m,
                        StagedAmount = 0m,
                        CommittedAmount = 0m,
                        DoneAmount = execution.FillAmount,
                        SodPrice = execution.FillPrice,
                        CostBasis = execution.FillPrice * execution.FillAmount,
                        TradingDay = execution.TradingDay
                    };
                    if (!next.ContainsKey(newPosition.Key))
                    {
                        next[newPosition.Key] = newPosition;
                    }
                    else
                    {
                        var oldPosition = next[newPosition.Key];
                        oldPosition.DoneAmount += execution.FillAmount;
                        oldPosition.CostBasis += execution.FillPrice * execution.FillAmount;
                    }
                    return next;
                });
            var lsnQuotes = new KafkaSpout("Quotes", cancellationToken);
            var obQuotes =
                lsnQuotes.WhenMessageReceived.Select(
                    kafkaEvent => JsonConvert.DeserializeObject<Quote[]>(kafkaEvent.Text)
                    );
            var obPositions =
                obSodHoldings
                    .Select(holdings =>
                    {
                        var newPositions = holdings
                            .Select(holding =>
                                new Position()
                                {
                                    Security = holding.Security,
                                    Custodian = holding.Custodian,
                                    SodAmount = holding.SodAmount,
                                    TargetAmount = holding.SodAmount,
                                    StagedAmount = holding.SodAmount,
                                    CommittedAmount = holding.SodAmount,
                                    DoneAmount = holding.SodAmount,
                                    SodPrice = holding.SodPrice,
                                    CostBasis = holding.CostBasis,
                                    TradingDay = holding.TradingDay
                                })
                            .ToArray();
                        return newPositions;
                    });
            var obPositions2 =
                obSodHoldings
                    .CombineLatest(obAggExecution,
                        (holdings, netExecutions) =>
                        {
                            var sodTradingDay = holdings.First().TradingDay;
                            var execTradingDay = netExecutions.Values.FirstOrDefault()?.TradingDay;
                            if ((!execTradingDay.HasValue) || (execTradingDay.Value != sodTradingDay))
                                netExecutions = new Dictionary<string, Position>();
                            var newPositions = holdings
                                .Select(holding =>
                                    new Position()
                                    {
                                        Security = holding.Security,
                                        Custodian = holding.Custodian,
                                        SodAmount = holding.SodAmount,
                                        TargetAmount = holding.TargetAmount,
                                        StagedAmount = holding.TargetAmount,
                                        CommittedAmount = holding.TargetAmount,
                                        DoneAmount = holding.SodAmount,
                                        SodPrice = holding.SodPrice,
                                        CostBasis = holding.CostBasis,
                                        TradingDay = holding.TradingDay
                                    })
                                .ToDictionary(x => x.Key, x => x.DeepClone());
                            foreach (var execPositionPair in netExecutions)
                            {
                                var execPosition = execPositionPair.Value;
                                if (!newPositions.ContainsKey(execPositionPair.Key))
                                {
                                    newPositions[execPositionPair.Key] = execPosition;
                                }
                                else
                                {
                                    var newPosition = newPositions[execPositionPair.Key];
                                    newPosition.DoneAmount += execPosition.DoneAmount;
                                    newPosition.CostBasis += execPosition.CostBasis;
                                }
                            }
                            return newPositions.Values.OrderBy(x => x.Key).ToArray();
                        });
            //obPositions2.Subscribe(
            //    netPositions =>
            //    {
            //        Console.WriteLine(
            //            $"At {netPositions.Max(x => x.TradingDay)} got {netPositions.Sum(x => x.DoneAmount)} shares costing {netPositions.Sum(x => x.CostBasis)}");
            //    },
            //    ex => Console.WriteLine($"Got Exception {ex.Message}"));
            var obExposures =
                obPositions2
                    .CombineLatest(obQuotes,
                        (positions, quotes) => new { positions, quotes })
                    .Sample(TimeSpan.FromMilliseconds(500))
                    .Select(tuple =>
                        (
                            from position in tuple.positions
                            group position by new { position.Security }
                            into grp
                            join quote in tuple.quotes
                            on grp.Key.Security equals quote.Security
                            let sodAmount = grp.Sum(x => x.SodAmount)
                            let sodPrice = grp.Max(x => x.SodPrice)
                            let targetAmount = grp.Sum(x => x.TargetAmount)
                            let doneAmount = grp.Sum(x => x.DoneAmount)
                            let costBasis = grp.Sum(x => x.CostBasis)
                            select new Exposure()
                            {
                                TradingDay = grp.Max(x=>x.TradingDay),
                                QuoteDate = quote.QuoteDate,
                                Security = grp.Key.Security,
                                SodAmount = sodAmount,
                                DoneAmount = doneAmount,
                                TargetAmount = targetAmount,
                                SodExposureUSD = quote.QuotePrice * sodAmount,
                                DoneExposureUSD = quote.QuotePrice * doneAmount,
                                TargetExposureUSD = quote.QuotePrice * targetAmount,
                                SodIntradayPLUSD = (quote.QuotePrice - sodPrice) * sodAmount,
                                DoneIntradayPLUSD = (quote.QuotePrice - sodPrice) * doneAmount,
                                TargetIntradayPLUSD = (quote.QuotePrice - sodPrice) * targetAmount,
                                AvgReturnPerDoneShareUSD = quote.QuotePrice - costBasis / doneAmount,
                                positions = grp.ToArray()
                            }).ToArray());
            //obExposures.Subscribe(
            //    rtPositions =>
            //    {
            //        Console.WriteLine(
            //            $"At {rtPositions.Max(x => x.QuoteDate)} got {rtPositions.Sum(x => x.DoneAmount)} shares valued at {rtPositions.Sum(x => x.DoneExposureUSD)}");
            //    },
            //    ex => Console.WriteLine($"Got Exception {ex.Message}"));
            return obExposures;
        }

        public static IObservable<string> SerializeExposures(IObservable<Exposure[]> exposuresObservable)
        {
            return exposuresObservable.Select(JsonConvert.SerializeObject);
        }
    }
}
