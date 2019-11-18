using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Web.Controllers.Abstractions;

namespace Cashflowio.Web.Controllers
{
    public class MoneyAccountController : CrudController<MoneyAccount>
    {
        public MoneyAccountController(IRepository repository) : base(repository)
        {
            FilterBy = (moneyAccount, avatarId) => moneyAccount.AvatarId == avatarId;
        }
    }
}