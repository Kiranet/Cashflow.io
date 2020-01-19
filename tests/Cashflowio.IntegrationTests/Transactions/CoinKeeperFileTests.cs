using Cashflowio.Infrastructure.Data;
using Xunit;

namespace Cashflowio.Tests.Transactions
{
    public class CoinKeeperFileTests
    {
        [Fact]
        public void ReadFile()
        {
            var transactions = RawTransactionFactory.ReadFromFile("./Assets/CoinKeeper.xlsx");
            Assert.NotEmpty(transactions);
            Assert.True(transactions.TrueForAll(t => t.IsValid()));
        }
    }
}