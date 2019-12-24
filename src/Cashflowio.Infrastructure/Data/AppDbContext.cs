using Cashflowio.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cashflowio.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<RawTransaction> RawTransactions { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<MoneyAccount> MoneyAccounts { get; set; }
        public DbSet<IncomeSource> IncomeSources { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Income> Income { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Concept> Concepts { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<CreditCharge> CreditCharges { get; set; }
        public DbSet<CreditPayment> CreditPayments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IncomeSource>().Ignore(x => x.GeneratedConcepts);
        }
    }
}