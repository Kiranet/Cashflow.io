using System.Linq;
using Cashflowio.Core.Entities;
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

        public IActionResult Grid()
        {
            var transactions = _repository.List<RawTransaction>();

            if (transactions.Any()) return View(transactions);

            var items = RawTransactionReader.GetAll(
                @"C:\Users\Jesus\source\repos\Cashflowio\Cashflowio\tests\Cashflowio.Tests\Assets\CoinKeeper.xlsx");

            _repository.AddRange(items);

            return View(_repository.List<RawTransaction>());
        }

        public IActionResult Pivot()
        {
            var transactions = _repository.List<RawTransaction>();

            if (transactions.Any()) return View(transactions);

            var items = RawTransactionReader.GetAll(
                @"C:\Users\Jesus\source\repos\Cashflowio\Cashflowio\tests\Cashflowio.Tests\Assets\CoinKeeper.xlsx");

            _repository.AddRange(items);

            return View(_repository.List<RawTransaction>());
        }
    }
}