using System;
using System.Collections.Generic;
using System.Linq;

namespace Cashflowio.Core.Entities
{
    public class CreditCharge : MoneyOutflow
    {
        public ICollection<CreditPayment> Payments { get; set; } = new List<CreditPayment>();
        public bool IsPaid => Payments.Any() && Math.Abs(Payments.Sum(x => x.Amount) - Amount) < 0.01;
    }
}