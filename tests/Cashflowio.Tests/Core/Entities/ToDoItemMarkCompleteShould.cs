using System.Linq;
using Cashflowio.Core.Events;
using Xunit;

namespace Cashflowio.Tests.Core.Entities
{
    public class RawTransactionMarkCompleteShould
    {
        [Fact]
        public void RaiseRawTransactionCompletedEvent()
        {
            var item = new RawTransactionBuilder().Build();


            Assert.Single(item.Events);
            Assert.IsType<RawTransactionProcessedEvent>(item.Events.First());
        }
    }
}
