using System;
using System.Collections.Generic;
using Cashflowio.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cashflowio.Web.Models
{
    public class AnalyticsViewModel
    {
        public SelectList Years { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public List<TransactionViewModel> Result { get; set; } = new List<TransactionViewModel>();
    }

    public class TransactionViewModel
    {
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public int Year => Date.Year;
        public int Month => Date.Month;
        public int Day => Date.Day;
        public string DayOfWeek => Date.DayOfWeek.ToString();
        public int Week => Date.GetWeekNumber();
    }
}