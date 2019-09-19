using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Interfaces
{
    public interface IHandle<T> where T : BaseDomainEvent
    {
        void Handle(T domainEvent);
    }
}