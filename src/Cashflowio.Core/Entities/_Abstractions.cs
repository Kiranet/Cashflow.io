using System;
using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Entities
{
    public abstract class Account : BaseEntity
    {
        public string Name { get; set; }
        public Currency Currency { get; set; } = Currency.Mxn;

        public int AvatarId { get; set; }
        public Avatar Avatar { get; set; }
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