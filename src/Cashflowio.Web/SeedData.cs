using Cashflowio.Core.Entities;
using Cashflowio.Infrastructure.Data;

namespace Cashflowio.Web
{
    public static class SeedData
    {
        public static void PopulateTestData(AppDbContext dbContext)
        {
            var transactions = dbContext.RawTransactions;
            foreach (var item in transactions)
            {
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();
            dbContext.RawTransactions.Add(new RawTransaction()
            {
            });
            dbContext.RawTransactions.Add(new RawTransaction()
            {
            });
            dbContext.SaveChanges();
        }

    }
}
