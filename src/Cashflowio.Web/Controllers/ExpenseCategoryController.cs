using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Web.Controllers.Abstractions;

namespace Cashflowio.Web.Controllers
{
    public class ExpenseCategoryController : CrudController<ExpenseCategory>
    {
        public ExpenseCategoryController(IRepository repository) : base(repository)
        {
            FilterBy = (expenseCategory, budgetCategoryId) =>
                expenseCategory.BudgetCategoryId == budgetCategoryId;
        }
    }
}