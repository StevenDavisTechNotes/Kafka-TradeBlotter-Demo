using System;

namespace KafktaListener.Models
{
    public class Quote
    {
        public string Key => $"{Security}-{QuoteDate:G}";

        public string Security { get; set; }

        public DateTime QuoteDate { get; set; }

        public decimal QuotePrice { get; set; }
    }
}
