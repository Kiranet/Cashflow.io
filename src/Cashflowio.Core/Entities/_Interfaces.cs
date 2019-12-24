namespace Cashflowio.Core.Entities
{
    public interface INameable
    {
        string Name { get; set; }
    }

    internal interface IMoneyOutflow
    {
        int SourceId { get; set; }
        MoneyAccount Source { get; set; }

        int DestinationId { get; set; }
        ExpenseCategory Destination { get; set; }

        int ConceptId { get; set; }
        Concept Concept { get; set; }
    }

    internal interface ICashflow
    {
        int SourceId { get; set; }
        MoneyAccount Source { get; set; }

        int DestinationId { get; set; }
        MoneyAccount Destination { get; set; }
    }
}