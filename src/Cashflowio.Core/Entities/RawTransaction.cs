using System;
using System.Collections.Generic;
using Cashflowio.Core.SharedKernel;
using FluentDateTime;

namespace Cashflowio.Core.Entities
{
    public class RawTransaction : BaseEntity
    {
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Tag { get; set; }
        public string Currency { get; set; }
        public string Note { get; set; }
        public bool IsProcessed { get; set; }

        public bool IsValid()
        {
            var hasPositiveAmount = Amount > 0;
            var hasValidDate = Date != default && Date.IsInPast();
            var hasType = !string.IsNullOrWhiteSpace(Type);
            var hasSource = !string.IsNullOrWhiteSpace(Source);
            var hasDestination = !string.IsNullOrWhiteSpace(Destination);
            var hasCurrency = !string.IsNullOrWhiteSpace(Currency);

            return new List<bool>
            {
                hasPositiveAmount, hasValidDate, hasType,
                hasSource, hasDestination, hasCurrency
            }.TrueForAll(x => x);
        }

        public RawTransaction Normalized()
        {
            foreach (var (oldValue, newValue) in new Dictionary<string, string>
            {
                {"Ã¡", "á"},
                {"Ã©", "é"},
                {"Ã­", "í"},
                {"Ã³", "ó"},
                {"Ãº", "ú"},
                {"Ã±", "ñ"},
                {"Ã¼", "ü"}
            })
            {
                Source = Source.Replace(oldValue, newValue).Trim();
                Destination = Destination.Replace(oldValue, newValue).Trim();
                Tag = Tag.Replace(oldValue, newValue).Trim();
                Note = Note.Replace(oldValue, newValue).Trim();
            }

            return this;
        }
    }
}