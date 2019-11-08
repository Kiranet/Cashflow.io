using System;
using System.Collections.Generic;

namespace Cashflowio.Core.Entities
{
    public class Concept : Transaction
    {
        public DayOfWeek PayDay { get; set; }

        public int DestinationId { get; set; }
        public MoneyAccount Destination { get; set; }

        public IEnumerable<DateTime> Dates { get; set; }
    }
}