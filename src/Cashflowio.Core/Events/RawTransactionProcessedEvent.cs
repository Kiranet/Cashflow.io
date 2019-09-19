using Cashflowio.Core.Entities;
using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Events
{
    public class RawTransactionProcessedEvent : BaseDomainEvent
    {
        public RawTransaction ProcessedItem { get; set; }

        public RawTransactionProcessedEvent(RawTransaction processedItem)
        {
            ProcessedItem = processedItem;
        }
    }
}