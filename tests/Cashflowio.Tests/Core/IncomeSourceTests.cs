using System;
using System.Collections.Generic;
using System.Linq;
using Cashflowio.Core.Entities;
using FluentDateTime;
using Newtonsoft.Json;
using Xunit;

namespace Cashflowio.Tests.Core
{
    public class IncomeSourceTests
    {
        [Fact]
        public void SetupAndGetIncome()
        {
            var dateOfReference = new DateTime(2019, 11, 1);
            var concepts = JsonConvert.SerializeObject(new List<IncomeBreakdown>
            {
                new IncomeBreakdown
                {
                    Amount = 4200,
                    Destination = FakeRepository.First(AccountType.Debit),
                    PayDay = DayOfWeek.Friday,
                    Description = "Sueldo",
                    StartDate = dateOfReference.FirstDayOfMonth(),
                    EndDate = dateOfReference.LastDayOfMonth(),
                    Recurrence = Recurrence.Weekly
                }
            });

            var incomeSource = new IncomeSource("Monobits")
            {
                Concepts = concepts
            };

            var incomes = incomeSource.GenerateIncome();

            Assert.Equal(5, incomes.Count);
            Assert.True(incomes.All(x => x.Date.DayOfWeek == DayOfWeek.Friday));
        }
    }
}