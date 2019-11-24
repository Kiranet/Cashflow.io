namespace Cashflowio.Core.Entities
{
    public class Income : Transaction
    {
        public int DestinationId { get; set; }
        public MoneyAccount Destination { get; set; }
    }
}