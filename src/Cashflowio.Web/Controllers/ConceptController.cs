using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Web.Controllers.Abstractions;

namespace Cashflowio.Web.Controllers
{
    public class ConceptController : CrudController<Concept>
    {
        public ConceptController(IRepository repository) : base(repository)
        {
        }
    }
}