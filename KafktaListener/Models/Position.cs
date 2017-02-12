using System;

namespace TickingViewSvc_Net.Models
{
    public class Position
    {
        public string Key => $"{Security}-{Custodian}-{ExecutingBroker}-{DisplayPurchaseDate}";

        public string Security { get; set; }
        public string Custodian { get; set; }

        public string ExecutingBroker { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string DisplayPurchaseDate => PurchaseDate?.ToString("yyyy MMMM dd") ?? "Today";

        public decimal SodAmount { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal StagedAmount { get; set; }
        public decimal CommittedAmount { get; set; }
        public decimal DoneAmount { get; set; }
        public decimal LeavesAmount => TargetAmount - DoneAmount;

        public decimal CostBasis { get; set; }
    }
}
