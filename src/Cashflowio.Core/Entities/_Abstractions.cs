using System;
using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Entities
{
    public abstract class Account : BaseEntity
    {
        public string Name { get; set; }
        public string Currency { get; set; }
    }

    public abstract class Transaction : BaseEntity
    {
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }

        public int? ExchangeRateId { get; set; }
        public ExchangeRate ExchangeRate { get; set; }
    }
}