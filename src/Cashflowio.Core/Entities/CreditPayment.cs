namespace Cashflowio.Core.Entities
{
    public class CreditPayment : Transaction, ICashflow
    {
        public int SourceId { get; set; }
        public MoneyAccount Source { get; set; }

        public int DestinationId { get; set; }
        public MoneyAccount Destination { get; set; }

        public int CreditChargeId { get; set; }
        public CreditCharge CreditCharge { get; set; }
    }
}