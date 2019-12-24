using Newtonsoft.Json;

namespace Cashflowio.Core.Entities
{
    public class CreditPayment : Transaction, ICashflow
    {
        public int? CreditChargeId { get; set; }

        [JsonIgnore] public CreditCharge CreditCharge { get; set; }
        public int SourceId { get; set; }
        public MoneyAccount Source { get; set; }

        public int DestinationId { get; set; }
        public MoneyAccount Destination { get; set; }
    }
}