namespace Cashflowio.Core.Entities
{
    public class Expense : Transaction, IMoneyOutflow
    {
        public int SourceId { get; set; }
        public MoneyAccount Source { get; set; }
        public int DestinationId { get; set; }
        public ExpenseCategory Destination { get; set; }
        public int ConceptId { get; set; }
        public Concept Concept { get; set; }
        public string Currency { get; set; }
    }
}