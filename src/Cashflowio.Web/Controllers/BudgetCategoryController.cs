using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Web.Controllers.Abstractions;

namespace Cashflowio.Web.Controllers
{
    public class BudgetCategoryController : CrudController<BudgetCategory>
    {
        public BudgetCategoryController(IRepository repository) : base(repository)
        {
        }
    }
}