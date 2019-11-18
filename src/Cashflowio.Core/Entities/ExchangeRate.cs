using System;
using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Entities
{
    public class ExchangeRate : BaseEntity
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public Currency Currency { get; set; } = Currency.Usd;
        public string Description { get; set; }
    }
}