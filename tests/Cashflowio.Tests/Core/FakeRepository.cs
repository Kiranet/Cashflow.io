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
            MoneyAccount.Cash("Dólares", Currency.Usd),
            MoneyAccount.Debit("Débito"),
            MoneyAccount.Credit("Crédito"),
            MoneyAccount.Savings("AFORE"),
            MoneyAccount.Savings("Ahorro"),
            MoneyAccount.Prepaid("Costco")
        };

        public static MoneyAccount FirstOrNull(AccountType? type) =>
            Accounts.FirstOrDefault(x => x.Type == type);

        public static MoneyAccount First(AccountType? type) => Accounts.First(x => x.Type == type);
    }
}