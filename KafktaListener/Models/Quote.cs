﻿using System;

namespace KafktaListener.Models
{
    public class Quote
    {
        public string Key => $"{Security}-{QuoteDate:G}";

        public string Security { get; set; }

        public DateTime QuoteDate { get; set; }

        public decimal QuotePrice { get; set; }
    }
    public class QuotesMessage
    {
        public string Type { get; set; }
        public Quote[] Data { get; set; }
    }

}
