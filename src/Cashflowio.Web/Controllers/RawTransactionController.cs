using Cashflowio.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cashflowio.Web.Controllers
{
    public class RawTransactionController : Controller
    {
        private readonly IRawTransactionService _service;

        public RawTransactionController(IRawTransactionService service)
        {
            _service = service;
        }

        public IActionResult Grid()
        {
            ViewData["Title"] = "Tabla";
            var isUpdateNeeded = Request.Query.Count > 0;
            return View(_service.QueryAll(isUpdateNeeded));
        }

        public IActionResult Pivot()
        {
            ViewData["Title"] = "Pivote";
            var isUpdateNeeded = Request.Query.Count > 0;
            return View(_service.QueryAll(isUpdateNeeded));
        }
    }
}