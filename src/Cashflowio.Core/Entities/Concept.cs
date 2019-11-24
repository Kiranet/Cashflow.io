using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cashflowio.Core.Entities
{
    public class Concept
    {
        public DayOfWeek PayDay { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Recurrence Recurrence { get; set; }

        public int DestinationId { get; set; }

        [JsonIgnore] public MoneyAccount Destination { get; set; }
        [JsonIgnore] public IEnumerable<DateTime> Dates { get; set; }
    }
}