using System.Collections.Generic;
using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Entities
{
    public class Avatar : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<MoneyAccount> MoneyAccounts { get; set; } = new List<MoneyAccount>();
    }
}