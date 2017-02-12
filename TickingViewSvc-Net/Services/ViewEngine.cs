using System;
using System.Collections.Generic;
using System.Linq;
using KafktaListener;
using TickingViewSvc_Net.Models;

namespace TickingViewSvc_Net.Services
{
    public interface IViewEngine
    {
        Exposure[] GetPositionView();
    }
    public class ViewEngine: IViewEngine
    {
        public class RandomPricer
        {
            private Random rand = new Random();
            public decimal CurrentPrice = 100.00m;
            public decimal TurnCrank()
            {
                return CurrentPrice = Math.Round((decimal)(100.00 * (1+rand.NextDouble()/10)),2);
            }
        }
        private Dictionary<string,RandomPricer> pricers;

        public ViewEngine()
        {
            positions = makePositions().ToArray();
            aggPositions = rollupPositions();
            pricers = securities.ToDictionary(x => x, x => new RandomPricer());

            var sodHoldingsListener = new KafkaSpout("SodHoldings");
            var executionListener = new KafkaSpout("Execution");
            var quotesListener = new KafkaSpout("Quotes");

            new DebugKafkaListener(sodHoldingsListener);
            new DebugKafkaListener(executionListener);
            new DebugKafkaListener(quotesListener);
        }

        public bool IsCompleted {get;} = false;
        private IEnumerable<Position> makePositions()
        {
            var rand = new Random();
            foreach (var security in securities)
            {
                foreach (var broker in new[] { "ITG", "GS-EQ", "JPMS" })
                {
                    foreach (var custodian in new[] { "CITI", "GSCO", "WFSE" })
                    {
                        foreach (var tradeDate in new DateTime?[] { new DateTime(2017, 1, 23), new DateTime(2017, 1, 24), null})
                        {
                            var activeTrade = tradeDate.HasValue;
                            var amt = rand.Next(20000);
                            yield return
                                new Position()
                                {
                                    Security = security,
                                    ExecutingBroker = broker,
                                    Custodian = custodian,
                                    PurchaseDate = tradeDate,
                                    TargetAmount = amt,
                                    StagedAmount = activeTrade ? Math.Floor(amt * 0.8m) : amt,
                                    CommittedAmount = activeTrade ? Math.Floor(amt * 0.6m) : amt,
                                    DoneAmount = activeTrade ? Math.Floor(amt * 0.4m) : amt
                                };
                        }
                    }
                }
            }
        }

        private Exposure[] rollupPositions()
        {
            var exposures = (
                from trade in positions
                group trade by new {trade.Security}
                into grp
                select new Exposure()
                {
                    Security = grp.Key.Security,
                    SodAmount = grp.Where(x=>x.PurchaseDate.HasValue).Sum(x => x.DoneAmount),
                    DoneAmount = grp.Sum(x => x.DoneAmount),
                    positions = grp.ToArray()
                }
            ).ToArray();
            return exposures;
        }


        private string[] securities = new[] {"FEYE", "EXAS", "TSLA"};
        private Position[] positions;
        private Exposure[] aggPositions;

        public Exposure[] GetPositionView()
        {
            var shareprices = pricers.ToDictionary(x=>x.Key,x=>x.Value.TurnCrank());
            var sodprice = 100.00m;
            var result = (
                from position in aggPositions
                let shareprice = shareprices[position.Security]
                select new Exposure()
                {
                    Security = position.Security,
                    SodAmount = position.SodAmount,
                    DoneAmount = position.DoneAmount,
                    TargetAmount = position.TargetAmount,

                    SodExposureUSD = position.SodAmount * shareprice,
                    DoneExposureUSD = position.DoneAmount * shareprice,
                    TargetExposureUSD = position.TargetAmount * shareprice,

                    SodPLUSD = position.SodAmount * (shareprice - sodprice),
                    DoneIntradayPLUSD = position.DoneAmount * (shareprice - sodprice),
                    TargetIntradayPLUSD = position.TargetAmount * (shareprice - sodprice),

                    positions = position.positions,
                }).ToArray();
            return result;
        }
    }
}
