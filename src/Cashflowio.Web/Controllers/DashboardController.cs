using System.Linq;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cashflowio.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IRepository _repository;

        public DashboardController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var moneyAccountsByName = _repository.List<MoneyAccount>().ToDictionary(x => x.Name, x => x);
            var transfers = _repository.List<RawTransaction>().Where(x => x.Type == nameof(Transfer)).Select(x =>
            {
                moneyAccountsByName.TryGetValue(x.Source, out var sourceFound);
                moneyAccountsByName.TryGetValue(x.Destination, out var destinationFound);

                var transfer = sourceFound?.TransferTo(destinationFound) ?? new Transfer();

                transfer.Amount = x.Amount;
                transfer.Date = x.Date;
                transfer.Description = x.Note;
                transfer.SourceId = sourceFound?.Id ?? 0;
                transfer.Source = sourceFound;
                transfer.DestinationId = destinationFound?.Id ?? 0;
                transfer.Destination = destinationFound;
                
                return transfer;
            });

            return View(transfers);
        }
    }
}