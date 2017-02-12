using System;
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
            var lsnQuotes = new KafkaSpout("Quotes", cancellationToken);
            var obQuotes =
                lsnQuotes.WhenMessageReceived.Select(
                    kafkaEvent => JsonConvert.DeserializeObject<Quote[]>(kafkaEvent.Text)
                    );
            var obPositions =
                obSodHoldings
                    .Select(holdings => (
                        from holding in holdings
                        select new Position()
                        {
                            Security = holding.Security,
                            Custodian = holding.Custodian,
                            ExecutingBroker = null,
                            PurchaseDate = holding.PurchaseDate,
                            SodAmount = holding.SodAmount,
                            TargetAmount = holding.SodAmount,
                            StagedAmount = holding.SodAmount,
                            CommittedAmount = holding.SodAmount,
                            DoneAmount = holding.SodAmount,
                            SodPrice = holding.SodPrice,
                            CostBasis = holding.CostBasis
                        }).ToArray());
            var obExposures =
                obPositions
                    .CombineLatest(obQuotes,
                        (positions, quotes) => new {positions, quotes})
                    .Sample(TimeSpan.FromMilliseconds(500))
                    .Select(tuple =>
                        (
                            from position in tuple.positions
                            group position by new {position.Security}
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
                                AvgReturnPerDoneShareUSD = quote.QuotePrice - costBasis/ doneAmount,
                                positions = grp.ToArray()
                            }).ToArray());
            //obExposures.Subscribe(
            //    rtPositions =>
            //    {
            //        Console.WriteLine(
            //            $"At {rtPositions.Max(x => x.QuoteDate)} got {rtPositions.Sum(x => x.DoneAmount)} shares valued at {rtPositions.Sum(x => x.DoneAmount)}");
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
