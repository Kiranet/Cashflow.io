using Cashflowio.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cashflowio.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ITransactionService _transactionService;

        public DashboardController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public IActionResult Index(int year)
        {
            return View(_transactionService.Query(year));
        }
    }
}