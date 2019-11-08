using System;
using Cashflowio.Core;
using Cashflowio.Core.Entities;
using Xunit;

namespace Cashflowio.Tests.Core
{
    public class MoneyAccountTests
    {
        [Theory]
        [InlineData(MoneyAccount.Type.Cash, MoneyAccount.Type.Debit, Transfer.Type.Deposit)]
        [InlineData(MoneyAccount.Type.Debit, MoneyAccount.Type.Cash, Transfer.Type.Withdrawal)]
        [InlineData(MoneyAccount.Type.Savings, MoneyAccount.Type.Cash, Transfer.Type.Withdrawal)]
        [InlineData(MoneyAccount.Type.Savings, MoneyAccount.Type.Debit, Transfer.Type.Withdrawal)]
        [InlineData(MoneyAccount.Type.Cash, MoneyAccount.Type.Savings, Transfer.Type.Saving)]
        [InlineData(MoneyAccount.Type.Debit, MoneyAccount.Type.Savings, Transfer.Type.Saving)]
        [InlineData(MoneyAccount.Type.Cash, MoneyAccount.Type.Prepaid, Transfer.Type.Recharge)]
        [InlineData(MoneyAccount.Type.Debit, MoneyAccount.Type.Prepaid, Transfer.Type.Recharge)]
        [InlineData(MoneyAccount.Type.Debit, MoneyAccount.Type.Credit, Transfer.Type.Payment)]
        public void MakeValidTransfers(MoneyAccount.Type source, MoneyAccount.Type destination, Transfer.Type expected)
        {
            var sourceAccount = FakeRepository.First(source);
            var destinationAccount = FakeRepository.First(destination);

            var transfer = sourceAccount.TransferTo(destinationAccount);

            Assert.Equal(expected, transfer.TransferType);
        }

        [Theory]
        [InlineData(MoneyAccount.Type.Credit, null)]
        [InlineData(MoneyAccount.Type.Prepaid, null)]
        [InlineData(MoneyAccount.Type.Cash, MoneyAccount.Type.Credit)]
        [InlineData(MoneyAccount.Type.Savings, MoneyAccount.Type.Credit)]
        public void MakeInvalidTransfers(MoneyAccount.Type? source, MoneyAccount.Type? destination)
        {
            var sourceAccount = FakeRepository.FirstOrNull(source);
            var destinationAccount = FakeRepository.FirstOrNull(destination);

            if (sourceAccount == null || destinationAccount == null)
            {
                foreach (var accountType in EnumUtils.GetTypedValues<MoneyAccount.Type>())
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