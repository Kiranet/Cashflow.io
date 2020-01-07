using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Entities
{
    public class ExpenseCategory : BaseEntity, INameable
    {
        public string Name { get; set; }
        public int? BudgetCategoryId { get; set; }
        public BudgetCategory BudgetCategory { get; set; }
    }
}