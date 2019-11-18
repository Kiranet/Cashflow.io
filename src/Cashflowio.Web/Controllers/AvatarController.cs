using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Web.Controllers.Abstractions;

namespace Cashflowio.Web.Controllers
{
    public class AvatarController : CrudController<Avatar>
    {
        public AvatarController(IRepository repository) : base(repository)
        {
        }
    }
}