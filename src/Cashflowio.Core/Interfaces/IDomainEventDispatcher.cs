using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Interfaces
{
    public interface IDomainEventDispatcher
    {
        void Dispatch(BaseDomainEvent domainEvent);
    }
}