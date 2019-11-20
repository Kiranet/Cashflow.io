namespace Cashflowio.Core.Entities
{
    public class Transfer : Transaction
    {
        public int SourceId { get; set; }
        public MoneyAccount Source { get; set; }

        public int DestinationId { get; set; }
        public MoneyAccount Destination { get; set; }

        public string Type { get; set; }
    }
}