namespace Cashflowio.Core.Entities
{
    public class Income : Transaction
    {
        public int SourceId { get; set; }
        public IncomeSource Source { get; set; }

        public int DestinationId { get; set; }
        public MoneyAccount Destination { get; set; }
    }
}