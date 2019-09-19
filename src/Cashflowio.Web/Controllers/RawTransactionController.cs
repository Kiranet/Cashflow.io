using Cashflowio.Core;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cashflowio.Web.Controllers
{
    public class RawTransactionController : Controller
    {
        private readonly IRepository _repository;

        public RawTransactionController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var items = _repository.List<RawTransaction>();
            return View(items);
        }

        public IActionResult Populate()
        {
            int recordsAdded = DatabasePopulator.PopulateDatabase(_repository);
            return Ok(recordsAdded);
        }
    }
}
