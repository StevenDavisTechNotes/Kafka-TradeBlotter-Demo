using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TickingViewSvc.Models;

namespace TickingViewSvc.Services
{
    public interface IViewEngine
    {
        ViewPosition[] GetPositionView();
    }
    public class ViewEngine: IViewEngine
    {
        public class RandomPricer
        {
            private Random rand = new Random();
            public decimal CurrentPrice = 100.00m;
            public decimal TurnCrank()
            {
                return CurrentPrice = 100.00m * (1 + rand.Next(100) / 1000m);
            }
        }
        private RandomPricer pricer = new RandomPricer();
        private ViewPosition[] holdings = new[] {
            new ViewPosition() { Key = "MSFT", Security = "MSFT", SODAmount = 10000m, StgAmount = 12000m, CmtAmount = 11000m, TgtAmount = 20000m },
            new ViewPosition() { Key = "IBM", Security = "IBM", SODAmount = 20000m, StgAmount = 22000m, CmtAmount = 21000m, TgtAmount = 40000m },
        };

        public ViewPosition[] GetPositionView()
        {
            var shareprice = pricer.TurnCrank();
            var sodprice = 100.00m;
            return holdings.Select(h => new ViewPosition()
            {
                Key = h.Key,
                Security = h.Security,
                SODAmount = h.SODAmount,
                TgtAmount = h.TgtAmount,
                StgAmount = h.StgAmount,
                CmtAmount = h.CmtAmount,

                SODUSDExposure = h.SODAmount * shareprice,
                TgtUSDExposure = h.TgtAmount * shareprice,
                StgUSDExposure = h.StgAmount * shareprice,
                CmtUSDExposure = h.CmtAmount * shareprice,

                SODIntradayPLUSD = h.SODAmount * (shareprice - sodprice),
                TgtIntradayPLUSD = h.TgtAmount * (shareprice - sodprice),
                StgIntradayPLUSD = h.StgAmount * (shareprice - sodprice),
                CmtIntradayPLUSD = h.CmtAmount * (shareprice - sodprice),

            }).ToArray();
        }
    }
}
