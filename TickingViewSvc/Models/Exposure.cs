namespace TickingViewSvc.Models
{
    public class Exposure
    {
        public string Key => $"{Security}";
        public string Security { get; set; }

        public decimal SODAmount { get; set; }
        public decimal TgtAmount { get; set; }
        public decimal StgAmount { get; set; }
        public decimal CmtAmount { get; set; }
        public decimal DoneAmount { get; set; }

        public decimal SODUSDExposure { get; set; }
        public decimal TgtUSDExposure { get; set; }
        public decimal StgUSDExposure { get; set; }
        public decimal CmtUSDExposure { get; set; }
        public decimal DoneUSDExposure { get; set; }

        public decimal SODIntradayPLUSD { get; set; }
        public decimal TgtIntradayPLUSD { get; set; }
        public decimal StgIntradayPLUSD { get; set; }
        public decimal CmtIntradayPLUSD { get; set; }
        public decimal DoneIntradayPLUSD { get; set; }
        public Position[] Positions { get; set; }
    }
}
