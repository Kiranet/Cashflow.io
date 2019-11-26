using Microsoft.AspNetCore.Mvc;

namespace Cashflowio.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}