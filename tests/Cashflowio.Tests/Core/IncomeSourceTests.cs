using System;
using System.Collections.Generic;
using System.Linq;
using Cashflowio.Core.Entities;
using FluentDateTime;
using Xunit;

namespace Cashflowio.Tests.Core
{
    public class IncomeSourceTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetupAndGetIncome(bool isFixed)
        {
            var incomeSource = isFixed ? IncomeSource.Fixed("Monobits") : IncomeSource.Variable("Monobits");

            var today = DateTime.Today;
            incomeSource.StartDate = today.FirstDayOfMonth();
            incomeSource.EndDate = today.LastDayOfMonth();
            incomeSource.Recurrence = Recurrence.Weekly;
            incomeSource.Concepts = new List<Concept>
            {
                new Concept
                {
                    Amount = 3300,
                    Destination = FakeRepository.First(MoneyAccount.Type.Debit),
                    PayDay = DayOfWeek.Thursday,
                    Description = "Sueldo"
                },
                new Concept
                {
                    Amount = 700,
                    Destination = FakeRepository.First(MoneyAccount.Type.Debit),
                    PayDay = DayOfWeek.Friday,
                    Description = "Sueldo"
                },
                new Concept
                {
                    Amount = 200,
                    Destination = FakeRepository.First(MoneyAccount.Type.Cash),
                    PayDay = DayOfWeek.Friday,
                    Description = "Sueldo"
                }
            };

            var incomes = incomeSource.MakeIncome();

            Assert.Equal(14, incomes.Count);
            Assert.True(incomes.All(x => x.IsFixed == isFixed));
        }
    }
}