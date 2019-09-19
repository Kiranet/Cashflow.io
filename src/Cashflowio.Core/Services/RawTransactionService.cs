using Ardalis.GuardClauses;
using Cashflowio.Core.Events;
using Cashflowio.Core.Interfaces;

namespace Cashflowio.Core.Services
{
    public class RawTransactionService : IHandle<RawTransactionProcessedEvent>
    {
        public void Handle(RawTransactionProcessedEvent domainEvent)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));

            // Do Nothing
        }
    }
}
