using System.Linq;
using Cashflowio.Infrastructure.Data;
using Xunit;

namespace Cashflowio.Tests.Transactions
{
    public class CoinKeeperTests
    {
        [Fact]
        public void ReadFile()
        {
            var transactions = RawTransactionFactory.ReadFromFile("./Assets/CoinKeeper.xlsx");
            Assert.NotEmpty(transactions);
            Assert.True(transactions.Sum(x => x.Amount) > 0);
        }
    }
}