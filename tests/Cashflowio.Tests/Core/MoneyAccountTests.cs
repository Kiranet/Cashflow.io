using System;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Utils;
using Xunit;

namespace Cashflowio.Tests.Core
{
    public class MoneyAccountTests
    {
        [Theory]
        [InlineData(AccountType.Cash, AccountType.Debit, TransferType.Deposit)]
        [InlineData(AccountType.Debit, AccountType.Cash, TransferType.Withdrawal)]
        [InlineData(AccountType.Savings, AccountType.Cash, TransferType.Withdrawal)]
        [InlineData(AccountType.Savings, AccountType.Debit, TransferType.Withdrawal)]
        [InlineData(AccountType.Cash, AccountType.Savings, TransferType.Saving)]
        [InlineData(AccountType.Debit, AccountType.Savings, TransferType.Saving)]
        [InlineData(AccountType.Cash, AccountType.Prepaid, TransferType.Recharge)]
        [InlineData(AccountType.Debit, AccountType.Prepaid, TransferType.Recharge)]
        [InlineData(AccountType.Debit, AccountType.Credit, TransferType.Payment)]
        public void MakeValidTransfers(AccountType source, AccountType destination, TransferType expected)
        {
            var sourceAccount = FakeRepository.First(source);
            var destinationAccount = FakeRepository.First(destination);

            var transfer = sourceAccount.TransferTo(destinationAccount);

            Assert.Equal(expected, transfer.Type);
        }

        [Theory]
        [InlineData(AccountType.Credit, null)]
        [InlineData(AccountType.Prepaid, null)]
        [InlineData(AccountType.Cash, AccountType.Credit)]
        [InlineData(AccountType.Savings, AccountType.Credit)]
        public void MakeInvalidTransfers(AccountType? source, AccountType? destination)
        {
            var sourceAccount = FakeRepository.FirstOrNull(source);
            var destinationAccount = FakeRepository.FirstOrNull(destination);

            if (sourceAccount == null || destinationAccount == null)
            {
                foreach (var accountType in EnumUtils.GetTypedValues<AccountType>())
                {
                    var auxAccount = FakeRepository.First(accountType);

                    if (sourceAccount == null)
                        Assert.Throws<Exception>(() => auxAccount.TransferTo(destinationAccount));
                    else
                        Assert.Throws<Exception>(() => sourceAccount.TransferTo(auxAccount));
                }
            }
            else
                Assert.Throws<Exception>(() => sourceAccount.TransferTo(destinationAccount));
        }
    }
}