using System;
using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Entities
{
    public class ExchangeRate : BaseEntity
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
        public Currency Currency { get; set; } = Currency.Dolar;
    }
    
    public enum Currency
    {
        Dolar
    }
}