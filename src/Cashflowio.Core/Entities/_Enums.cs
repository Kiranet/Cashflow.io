namespace Cashflowio.Core.Entities
{
    public enum Currency
    {
        MXN,
        USD
    }

    public enum Recurrence
    {
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public enum AccountType
    {
        Cash,
        Debit,
        Credit,
        Savings,
        Prepaid
    }

    public enum TransferType
    {
        Cashflow,
        Deposit,
        Withdrawal,
        Saving,
        Recharge,
        Exchange,
        Other
    }
}