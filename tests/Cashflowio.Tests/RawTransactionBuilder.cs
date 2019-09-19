using Cashflowio.Core.Entities;

namespace Cashflowio.Tests
{
    public class RawTransactionBuilder
    {
        private readonly RawTransaction _transaction = new RawTransaction();

        public RawTransactionBuilder Id(int id)
        {
            _transaction.Id = id;
            return this;
        }

        public RawTransaction Build() => _transaction;
    }
}