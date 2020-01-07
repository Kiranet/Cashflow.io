using System.Collections.Generic;
using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Entities
{
    public class BudgetCategory : BaseEntity, INameable
    {
        public string Name { get; set; }
        public ICollection<ExpenseCategory> ExpenseCategories { get; set; } = new List<ExpenseCategory>();
    }
}