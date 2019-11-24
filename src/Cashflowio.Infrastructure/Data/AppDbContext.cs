using Cashflowio.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cashflowio.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IncomeSource>().Ignore(x => x.GeneratedConcepts);
        }

        public DbSet<RawTransaction> RawTransactions { get; set; }
        public DbSet<Binnacle> Binnacle { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<MoneyAccount> MoneyAccounts { get; set; }
        public DbSet<IncomeSource> IncomeSources { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
    }
}