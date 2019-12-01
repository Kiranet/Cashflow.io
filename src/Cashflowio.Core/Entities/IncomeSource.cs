using System;
using System.Collections.Generic;
using System.Linq;
using Cashflowio.Core.Extensions;
using Newtonsoft.Json;

namespace Cashflowio.Core.Entities
{
    public class IncomeSource : Account
    {
        public IncomeSource()
        {
            Currency = Entities.Currency.MXN.ToString();
        }

        public IncomeSource(string name, Currency currency = Entities.Currency.MXN)
        {
            Name = name;
            Currency = currency.ToString();
        }

        public string Concepts { get; set; }

        [JsonIgnore]
        public IEnumerable<Income> Income { get; set; }
        
        public List<IncomeBreakdown> GeneratedConcepts => JsonConvert.DeserializeObject<List<IncomeBreakdown>>(Concepts);

        public List<Income> GenerateIncome()
        {
            var makeIncome = new List<Income>();
            foreach (var concept in GeneratedConcepts)
            {
                var endDate = concept.EndDate ?? DateTime.Now;
                var startDate = concept.StartDate.GetNextWeekday(concept.PayDay);
                concept.Dates = startDate.RangeTo(endDate, concept.Recurrence);

                makeIncome.AddRange(concept.Dates.Select(date => new Income
                {
                    Date = date,
                    Amount = concept.Amount,
                    Description = concept.Description,
                    SourceId = Id,
                    DestinationId = concept.DestinationId
                }));
            }

            return makeIncome;
        }
    }
}