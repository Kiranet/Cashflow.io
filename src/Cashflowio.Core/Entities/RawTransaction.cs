using System;
using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Entities
{
    public class RawTransaction : BaseEntity
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Tag { get; set; }
        public string Note { get; set; }
        public string Recurrence { get; set; }
        public string CurrencyOfConversion { get; set; }
        public double AmountConverted { get; set; }
        public string Currency { get; set; }
    }
}