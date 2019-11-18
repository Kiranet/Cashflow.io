using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Web.Controllers.Abstractions;

namespace Cashflowio.Web.Controllers
{
    public class ExchangeRateController : CrudController<ExchangeRate>
    {
        public ExchangeRateController(IRepository repository) : base(repository)
        {
        }
    }
}