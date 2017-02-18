using System;

namespace TickingViewSvc_Net.Models
{
    public class Position
    {
        public string Key => $"{Security}";
        public int TradingDay { get; set; }

        public string Security { get; set; }
        public string Custodian { get; set; }

        public decimal SodAmount { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal StagedAmount { get; set; }
        public decimal CommittedAmount { get; set; }
        public decimal DoneAmount { get; set; }
        public decimal LeavesAmount => TargetAmount - DoneAmount;

        public decimal CostBasis { get; set; }
        public decimal SodPrice { get; set; }

        public Position()
        {
        }

        public Position DeepClone()
        {
            var src = this;
            return new Position() {
                Security = src.Security,
                Custodian = src.Custodian,
                SodAmount = src.SodAmount,
                TargetAmount = src.SodAmount,
                StagedAmount = src.SodAmount,
                CommittedAmount = src.SodAmount,
                DoneAmount = src.SodAmount,
                SodPrice = src.SodPrice,
                CostBasis = src.CostBasis,
                TradingDay = src.TradingDay
            };
        }
    }
}
