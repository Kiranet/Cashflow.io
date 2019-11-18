using System;
using System.Collections.Generic;
using Cashflowio.Core.Entities;
using FluentDateTime;

namespace Cashflowio.Core.Extensions
{
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