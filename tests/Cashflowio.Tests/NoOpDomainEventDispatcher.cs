using Cashflowio.Core.Interfaces;
using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Tests
{
    public class NoOpDomainEventDispatcher : IDomainEventDispatcher
    {
        public void Dispatch(BaseDomainEvent domainEvent) { }
    }
}
