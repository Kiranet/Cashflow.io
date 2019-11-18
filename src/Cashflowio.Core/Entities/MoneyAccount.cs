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
            Type = accountType.ToString();
            Name = name;
            Currency = currency.ToString();
        }

        public string Type { get; set; }

        #region Factory Methods

        public static MoneyAccount InstanceOf(AccountType accountType, string name, Currency currency) =>
            new MoneyAccount(accountType, name, currency);

        public static MoneyAccount Cash(string name, Currency currency = Entities.Currency.MXN) =>
            InstanceOf(AccountType.Cash, name, currency);

        public static MoneyAccount Debit(string name, Currency currency = Entities.Currency.MXN) =>
            InstanceOf(AccountType.Debit, name, currency);

        public static MoneyAccount Credit(string name, Currency currency = Entities.Currency.MXN) =>
            InstanceOf(AccountType.Credit, name, currency);

        public static MoneyAccount Savings(string name, Currency currency = Entities.Currency.MXN) =>
            InstanceOf(AccountType.Savings, name, currency);

        public static MoneyAccount Prepaid(string name, Currency currency = Entities.Currency.MXN) =>
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

            transfer.Type = GetValidType(transfer.Destination).ToString();

            return transfer;
        }

        private void Validate(MoneyAccount destination)
        {
            var isNotAllowedSource = Type == AccountType.Credit.ToString() || Type == AccountType.Prepaid.ToString();
            var isNotAllowedForCredit =
                destination.Type == AccountType.Credit.ToString() && Type != AccountType.Debit.ToString();

            if (isNotAllowedSource || isNotAllowedForCredit)
                throw new Exception($"{Type} to {destination.Type} is not valid.");
        }

        private TransferType GetValidType(MoneyAccount destination)
        {
            if (Type == AccountType.Cash.ToString() && destination.Type == AccountType.Debit.ToString())
                return TransferType.Deposit;

            if (destination.Type == AccountType.Savings.ToString())
                return TransferType.Saving;

            if (destination.Type == AccountType.Prepaid.ToString())
                return TransferType.Recharge;

            if (destination.Type == AccountType.Credit.ToString())
                return TransferType.Payment;

            if (Type == AccountType.Savings.ToString() || Type == AccountType.Debit.ToString())
                return TransferType.Withdrawal;

            return TransferType.Other;
        }
    }
}