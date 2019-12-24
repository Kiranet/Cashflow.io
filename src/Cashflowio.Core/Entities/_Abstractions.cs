using System;
using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Entities
{
    public abstract class Account : BaseEntity, INameable
    {
        public string Currency { get; set; }
        public string Name { get; set; }
    }

    public abstract class Transaction : BaseEntity
    {
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }

        public int? ExchangeRateId { get; set; }
        public ExchangeRate ExchangeRate { get; set; }

        public int RawTransactionId { get; set; }
        public RawTransaction RawTransaction { get; set; }
    }

    public abstract class MoneyOutflow : Transaction, IMoneyOutflow
    {
        public int SourceId { get; set; }
        public MoneyAccount Source { get; set; }
        public int DestinationId { get; set; }
        public ExpenseCategory Destination { get; set; }
        public int ConceptId { get; set; }
        public Concept Concept { get; set; }
    }
}