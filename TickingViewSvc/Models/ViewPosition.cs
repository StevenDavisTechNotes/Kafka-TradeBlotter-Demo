using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TickingViewSvc.Models
{
    public class ViewPosition
    {
        public string Key { get; set; }
        public string Security { get; set; }

        public decimal SODAmount { get; set; }
        public decimal TgtAmount { get; set; }
        public decimal StgAmount { get; set; }
        public decimal CmtAmount { get; set; }

        public decimal SODUSDExposure { get; set; }
        public decimal TgtUSDExposure { get; set; }
        public decimal StgUSDExposure { get; set; }
        public decimal CmtUSDExposure { get; set; }

        public decimal SODIntradayPLUSD { get; set; }
        public decimal TgtIntradayPLUSD { get; set; }
        public decimal StgIntradayPLUSD { get; set; }
        public decimal CmtIntradayPLUSD { get; set; }
    }
}
