using System.Linq;
using Cashflowio.Infrastructure.Data;
using Xunit;

namespace Cashflowio.Tests.Integration.Data
{
    public class CoinKeeperFileTests
    {
        [Fact]
        public void TestFile()
        {
            var transactions = RawTransactionReader.GetAll(
                @"C:\Users\Jesus\source\repos\Cashflowio\Cashflowio\tests\Cashflowio.Tests\Assets\CoinKeeper.xlsx");

            Assert.NotEmpty(transactions);
            Assert.True(transactions.Sum(x => x.Amount) > 0);
        }
    }
}