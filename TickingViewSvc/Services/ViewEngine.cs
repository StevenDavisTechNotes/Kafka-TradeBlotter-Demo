using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TickingViewSvc.Models;

namespace TickingViewSvc.Services
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
        }
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
                    SODAmount = grp.Where(x=>x.PurchaseDate.HasValue).Sum(x => x.DoneAmount),
                    TgtAmount = grp.Sum(x => x.TargetAmount),
                    StgAmount = grp.Sum(x => x.StagedAmount),
                    CmtAmount = grp.Sum(x => x.CommittedAmount),
                    DoneAmount = grp.Sum(x => x.DoneAmount),
                    Positions = grp.ToArray()
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
                    SODAmount = position.SODAmount,
                    TgtAmount = position.TgtAmount,
                    StgAmount = position.StgAmount,
                    CmtAmount = position.CmtAmount,
                    DoneAmount = position.DoneAmount,
                    Positions = position.Positions,

                    SODUSDExposure = position.SODAmount * shareprice,
                    TgtUSDExposure = position.TgtAmount * shareprice,
                    StgUSDExposure = position.StgAmount * shareprice,
                    CmtUSDExposure = position.CmtAmount * shareprice,
                    DoneUSDExposure = position.DoneAmount * shareprice,

                    SODIntradayPLUSD = position.SODAmount * (shareprice - sodprice),
                    TgtIntradayPLUSD = position.TgtAmount * (shareprice - sodprice),
                    StgIntradayPLUSD = position.StgAmount * (shareprice - sodprice),
                    CmtIntradayPLUSD = position.CmtAmount * (shareprice - sodprice),
                    DoneIntradayPLUSD = position.DoneAmount * (shareprice - sodprice),

                }).ToArray();
            return result;
        }
    }
}
