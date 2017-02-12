using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using KafktaListener;
using KafktaListener.Models;
using Newtonsoft.Json;
using TickingViewSvc_Net.Models;

namespace TradingAnalytics.Services
{
    public class ValuedRTPositionService
    {
        public ValuedRTPositionService()
        {
            var lsnSodHoldings = new KafkaSpout("SodHoldings");
            var obSodHoldings =
                lsnSodHoldings.WhenMessageReceived.Select(
                    kafkaEvent => JsonConvert.DeserializeObject<SodHolding[]>(kafkaEvent.Text)
                    );
            var lsnExecution = new KafkaSpout("Execution");
            var obExecution =
                lsnExecution.WhenMessageReceived.Select(
                    kafkaEvent => JsonConvert.DeserializeObject<Execution>(kafkaEvent.Text)
                    );
            var lsnQuotes = new KafkaSpout("Quotes");
            var obQuotes =
                lsnQuotes.WhenMessageReceived.Select(
                    kafkaEvent => JsonConvert.DeserializeObject<Quote[]>(kafkaEvent.Text)
                    );
            var obPositions =
                obSodHoldings
                    .Select(holdings => (
                        from position in holdings
                        select new Position()
                        {
                            Security = position.Security,
                            Custodian = position.Custodian,
                            ExecutingBroker = null,
                            PurchaseDate = position.PurchaseDate,
                            SodAmount = position.SodAmount,
                            TargetAmount = position.SodAmount,
                            StagedAmount = position.SodAmount,
                            CommittedAmount = position.SodAmount,
                            DoneAmount = position.SodAmount,
                            CostBasis = position.CostBasis
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
                                SodPLUSD = quote.QuotePrice * sodAmount - costBasis,
                                DoneIntradayPLUSD = quote.QuotePrice * doneAmount - costBasis,
                                TargetIntradayPLUSD = quote.QuotePrice * targetAmount - costBasis,
                                positions = grp.ToArray()
                            }).ToArray());
            obExposures.Subscribe(
                rtPositions =>
                {
                    Console.WriteLine(
                        $"At {rtPositions.Max(x => x.QuoteDate)} got {rtPositions.Sum(x => x.DoneAmount)} shares valued at {rtPositions.Sum(x => x.DoneAmount)}");
                },
                ex => Console.WriteLine($"Got Exception {ex.Message}"));
        }
    }
}
