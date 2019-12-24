namespace Cashflowio.Core.Entities
{
    public class Transfer : Transaction, ICashflow
    {
        public string Type { get; set; }
        public int SourceId { get; set; }
        public MoneyAccount Source { get; set; }

        public int DestinationId { get; set; }
        public MoneyAccount Destination { get; set; }
    }
}