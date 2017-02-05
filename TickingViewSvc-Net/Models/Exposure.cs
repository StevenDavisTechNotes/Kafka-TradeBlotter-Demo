namespace TickingViewSvc_Net.Models
{
    public class Exposure
    {
        public string key => $"{security}";
        public string security { get; set; }

        public decimal sodAmount { get; set; }
        public decimal cmtAmount { get; set; }

        public decimal sodUSDExposure { get; set; }
        public decimal cmtUSDExposure { get; set; }

        public decimal sodIntradayPLUSD { get; set; }
        public decimal cmtIntradayPLUSD { get; set; }
        public Position[] positions { get; set; }
    }
}
