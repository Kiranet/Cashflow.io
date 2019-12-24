using System.Collections.Generic;
using System.Linq;
using Cashflowio.Core.Entities;

namespace Cashflowio.Tests.Core
{
    public static class FakeRepository
    {
        private static readonly List<MoneyAccount> Accounts = new List<MoneyAccount>
        {
            MoneyAccount.Cash("Billetera"),
            MoneyAccount.Cash("Dólares", Currency.USD),
            MoneyAccount.Debit("Débito"),
            MoneyAccount.Credit("Crédito"),
            MoneyAccount.Savings("AFORE"),
            MoneyAccount.Savings("Ahorro"),
            MoneyAccount.Prepaid("Costco")
        };

        public static MoneyAccount FirstOrNull(AccountType? type)
        {
            return Accounts.FirstOrDefault(x => x.Type == type?.ToString());
        }

        public static MoneyAccount First(AccountType? type)
        {
            return Accounts.First(x => x.Type == type?.ToString());
        }
    }
}