using System;
using System.Collections.Generic;
using System.Linq;
using FluentDateTime;

namespace Cashflowio.Core.Entities
{
    public class IncomeSource : Account
    {
        private IncomeSource(string name, Currency currency)
        {
            Name = name;
            Currency = currency;
        }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Recurrence Recurrence { get; set; }
        public bool IsFixed { get; set; } = true;

        public List<Concept> Concepts { get; set; } = new List<Concept>();

        public static IncomeSource Fixed(string name, Currency currency = Currency.Mxn)
        {
            return new IncomeSource(name, currency) {IsFixed = true};
        }

        public static IncomeSource Variable(string name, Currency currency = Currency.Mxn)
        {
            return new IncomeSource(name, currency) {IsFixed = false};
        }

        public List<Income> MakeIncome()
        {
            var endDate = EndDate ?? DateTime.Now;

            Concepts.ToList().ForEach(concept =>
            {
                var startDate = StartDate.GetNextWeekday(concept.PayDay);
                concept.Dates = startDate.RangeTo(endDate, Recurrence);
            });

            return Concepts.Select(concept =>
            {
                return concept.Dates.Select(date => new Income
                {
                    Date = date,
                    Amount = concept.Amount,
                    IsFixed = IsFixed,
                    Description = concept.Description,
                    Source = concept,
                    Destination = concept.Destination,
                    ExchangeRate = concept.ExchangeRate
                });
            }).SelectMany(x => x).ToList();
        }
    }

    public static class DateTimeExtensions
    {
        public static IEnumerable<DateTime> RangeTo(this DateTime startDate, DateTime endDate,
            Recurrence recurrence)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.GetNext(recurrence))
                yield return date;
        }

        public static DateTime GetNext(this DateTime date, Recurrence recurrence)
        {
            return recurrence switch
            {
                Recurrence.Daily => date.NextDay(),
                Recurrence.Weekly => date.WeekAfter(),
                Recurrence.Monthly => date.NextMonth(),
                Recurrence.Yearly => date.NextYear(),
                _ => throw new ArgumentOutOfRangeException(nameof(recurrence), recurrence, null)
            };
        }

        public static DateTime GetNextWeekday(this DateTime start, DayOfWeek day)
        {
            var daysToAdd = ((int) day - (int) start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }
    }
}