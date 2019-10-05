using System.Linq;
using Cashflowio.Core;
using Cashflowio.Core.Interfaces;
using Cashflowio.Infrastructure.Data;
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
            var items =  RawTransactionReader.GetAll(
                @"C:\Users\Jesus\source\repos\Cashflowio\Cashflowio\tests\Cashflowio.Tests\Assets\CoinKeeper.xlsx");

            return View(items.Where(x => x.Date.Month == 4 && x.Date.Year == 2017));
        }

        public IActionResult Populate()
        {
            int recordsAdded = DatabasePopulator.PopulateDatabase(_repository);
            return Ok(recordsAdded);
        }
    }
}
