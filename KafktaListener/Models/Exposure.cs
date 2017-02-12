using System;

namespace TickingViewSvc_Net.Models
{
    public class Exposure
    {
        public string Key => $"{Security}";
        public DateTime QuoteDate { get; set; }
        public string Security { get; set; }

        public decimal SodAmount { get; set; }
        public decimal DoneAmount { get; set; }
        public decimal TargetAmount { get; set; }

        public decimal SodExposureUSD { get; set; }
        public decimal DoneExposureUSD { get; set; }
        public decimal TargetExposureUSD { get; set; }

        public decimal SodIntradayPLUSD { get; set; }
        public decimal DoneIntradayPLUSD { get; set; }
        public decimal TargetIntradayPLUSD { get; set; }

        public decimal AvgReturnPerDoneShareUSD { get; set; }

        public Position[] positions { get; set; }
    }
}
