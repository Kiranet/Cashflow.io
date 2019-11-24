using System.Linq;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Web.Controllers.Abstractions;

namespace Cashflowio.Web.Controllers
{
    public class IncomeSourceController : CrudController<IncomeSource>
    {
        public IncomeSourceController(IRepository repository) : base(repository)
        {
            FilterBy = (incomeSource, moneyAccountId) =>
                incomeSource.GeneratedConcepts.All(x => x.DestinationId == moneyAccountId);
        }
    }
}