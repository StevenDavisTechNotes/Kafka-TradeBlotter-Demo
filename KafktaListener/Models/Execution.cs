using System;

namespace KafktaListener.Models
{
    public class Execution
    {
        public string Key => $"{Security}-{Custodian}-{ExecutingBroker}-{DisplayPurchaseDate}";
        public int TradingDay { get; set; }

        public string Security { get; set; }
        public string Custodian { get; set; }
        public string ExecutingBroker { get; set; }

        public DateTime? PurchaseDate { get; set; }
        public string DisplayPurchaseDate => PurchaseDate?.ToString("yyyy MMMM dd") ?? "Today";

        public decimal FillAmount { get; set; }
        public decimal FillPrice { get; set; }
    }
}
