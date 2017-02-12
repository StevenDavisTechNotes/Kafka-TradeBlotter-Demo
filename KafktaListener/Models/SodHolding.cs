using System;

namespace KafktaListener.Models
{
    public class SodHolding
    {
        public string Key => $"{Security}-{Custodian}-{DisplayPurchaseDate}";

        public string Security { get; set; }
        public string Custodian { get; set; }

        public DateTime? PurchaseDate { get; set; }
        public string DisplayPurchaseDate => PurchaseDate?.ToString("yyyy MMMM dd") ?? "Today";

        public decimal SodAmount { get; set; }
        public decimal SodPrice { get; set; }
        public decimal CostBasis { get; set; }
    }
}
