using System;

namespace Cashflowio.Core.Entities
{
    public class MoneyAccount : Account
    {
        public MoneyAccount()
        {
        }

        private MoneyAccount(Type type, string name, Currency currency)
        {
            AccountType = type;
            Name = name;
            Currency = currency;
        }

        public Type AccountType { get; private set; }

        #region Factory Methods

        public static MoneyAccount InstanceOf(Type type, string name, Currency currency) =>
            new MoneyAccount(type, name, currency);

        public static MoneyAccount Cash(string name, Currency currency = Currency.Mxn) =>
            InstanceOf(Type.Cash, name, currency);

        public static MoneyAccount Debit(string name, Currency currency = Currency.Mxn) =>
            InstanceOf(Type.Debit, name, currency);

        public static MoneyAccount Credit(string name, Currency currency = Currency.Mxn) =>
            InstanceOf(Type.Credit, name, currency);

        public static MoneyAccount Savings(string name, Currency currency = Currency.Mxn) =>
            InstanceOf(Type.Savings, name, currency);

        public static MoneyAccount Prepaid(string name, Currency currency = Currency.Mxn) =>
            InstanceOf(Type.Prepaid, name, currency);

        #endregion

        public Transfer TransferTo(MoneyAccount destination)
        {
            var transfer = new Transfer
            {
                Source = this,
                Destination = destination
            };

            Validate(transfer.Destination);

            transfer.TransferType = GetValidType(transfer.Destination);

            return transfer;
        }

        private void Validate(MoneyAccount destination)
        {
            var isNotAllowedSource = AccountType == Type.Credit || AccountType == Type.Prepaid;
            var isNotAllowedForCredit = destination.AccountType == Type.Credit && AccountType != Type.Debit;

            if (isNotAllowedSource || isNotAllowedForCredit)
                throw new Exception($"{AccountType} to {destination.AccountType} is not valid.");
        }

        private Transfer.Type GetValidType(MoneyAccount destination)
        {
            if (AccountType == Type.Cash && destination.AccountType == Type.Debit)
                return Transfer.Type.Deposit;

            switch (destination.AccountType)
            {
                case Type.Savings:
                    return Transfer.Type.Saving;
                case Type.Prepaid:
                    return Transfer.Type.Recharge;
                case Type.Credit:
                    return Transfer.Type.Payment;
                default:
                {
                    if (AccountType == Type.Savings || AccountType == Type.Debit)
                        return Transfer.Type.Withdrawal;

                    return Transfer.Type.Unknown;
                }
            }
        }

        public enum Type
        {
            Cash,
            Debit,
            Credit,
            Savings,
            Prepaid
        }
    }
}