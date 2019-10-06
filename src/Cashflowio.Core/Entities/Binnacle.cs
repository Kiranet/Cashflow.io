using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Entities
{
    public class Binnacle : BaseEntity
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string Good { get; set; }
        public string Bad { get; set; }
        public string Advice { get; set; }
        public string Review { get; set; }
    }
}