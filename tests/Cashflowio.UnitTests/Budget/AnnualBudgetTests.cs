using System;
using System.Collections.Generic;
using FluentDateTime;
using Xunit;

namespace Cashflowio.UnitTests.Budget
{
    public class AnnualBudgetTests
    {
        [Fact]
        public void Configure_AnnualBudget()
        {
            var sut = new AnnualBudget(2020);
            sut.MakeForecast(
                new[]
                {
                    new Forecast("Jesús", 0, 4200.0 * 52)
                    {
                        Savings = new[]
                        {
                            new Saving("AFORE", 100.0, DayOfWeek.Monday, TimeSpan.FromDays(7))
                        }
                    },
                    new Forecast("Elisa", 10000, 6000.0 * 25)
                }
            );

            // sut.MonthlyBudget(1, )
        }
    }

    public class Saving
    {
        public Saving(string concept, double amount, DayOfWeek onDay, TimeSpan every)
        {
            throw new NotImplementedException();
        }
    }

    public class Forecast
    {
        public Forecast(string name, double initialBalance, double expectedIncome)
        {
            Name = name;
            InitialBalance = initialBalance;
            ExpectedIncome = expectedIncome;
        }

        public string Name { get; set; }

        public double InitialBalance { get; set; }

        public double ExpectedIncome { get; set; }

        public ICollection<Saving> Savings { get; set; } = new List<Saving>();
    }

    public class AnnualBudget
    {
        public AnnualBudget(int year)
        {
            Year = year;
        }

        public int Year { get; set; }

        public double NetWorth => 0;

        public void MakeForecast(Forecast[] forecasts)
        {
            Forecasts = forecasts;
        }

        public ICollection<Forecast> Forecasts { get; set; }
    }
}