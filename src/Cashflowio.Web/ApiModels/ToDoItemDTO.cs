using System.ComponentModel.DataAnnotations;
using Cashflowio.Core.Entities;

namespace Cashflowio.Web.ApiModels
{
    // Note: doesn't expose events or behavior
    public class RawTransactionDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; private set; }

        public static RawTransactionDTO FromRawTransaction(RawTransaction item)
        {
            return new RawTransactionDTO()
            {
                Id = item.Id,
            };
        }
    }
}
