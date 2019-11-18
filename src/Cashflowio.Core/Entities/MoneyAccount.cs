using System;

namespace Cashflowio.Core.Entities
{
    public class MoneyAccount : Account
    {
        public MoneyAccount()
        {
        }

        private MoneyAccount(AccountType accountType, string name, Currency currency)
        {
            Type = accountType;
            Name = name;
            Currency = currency;
        }

        public AccountType Type { get; set; }

        #region Factory Methods

        public static MoneyAccount InstanceOf(AccountType accountType, string name, Currency currency) =>
            new MoneyAccount(accountType, name, currency);

        public static MoneyAccount Cash(string name, Currency currency = Currency.Mxn) =>
            InstanceOf(AccountType.Cash, name, currency);

        public static MoneyAccount Debit(string name, Currency currency = Currency.Mxn) =>
            InstanceOf(AccountType.Debit, name, currency);

        public static MoneyAccount Credit(string name, Currency currency = Currency.Mxn) =>
            InstanceOf(AccountType.Credit, name, currency);

        public static MoneyAccount Savings(string name, Currency currency = Currency.Mxn) =>
            InstanceOf(AccountType.Savings, name, currency);

        public static MoneyAccount Prepaid(string name, Currency currency = Currency.Mxn) =>
            InstanceOf(AccountType.Prepaid, name, currency);

        #endregion

        public Transfer TransferTo(MoneyAccount destination)
        {
            var transfer = new Transfer
            {
                Source = this,
                Destination = destination
            };

            Validate(transfer.Destination);

            transfer.Type = GetValidType(transfer.Destination);

            return transfer;
        }

        private void Validate(MoneyAccount destination)
        {
            var isNotAllowedSource = Type == AccountType.Credit || Type == AccountType.Prepaid;
            var isNotAllowedForCredit = destination.Type == AccountType.Credit && Type != AccountType.Debit;

            if (isNotAllowedSource || isNotAllowedForCredit)
                throw new Exception($"{Type} to {destination.Type} is not valid.");
        }

        private TransferType GetValidType(MoneyAccount destination)
        {
            if (Type == AccountType.Cash && destination.Type == AccountType.Debit)
                return Entities.TransferType.Deposit;

            switch (destination.Type)
            {
                case AccountType.Savings:
                    return Entities.TransferType.Saving;
                case AccountType.Prepaid:
                    return Entities.TransferType.Recharge;
                case AccountType.Credit:
                    return Entities.TransferType.Payment;
                default:
                {
                    if (Type == AccountType.Savings || Type == AccountType.Debit)
                        return Entities.TransferType.Withdrawal;

                    return Entities.TransferType.Other;
                }
            }
        }
    }
}