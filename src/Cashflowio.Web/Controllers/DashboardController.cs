using System;
using System.Linq;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Cashflowio.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IRepository _repository;

        public DashboardController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Transfer()
        {
            var moneyAccountsByName = _repository.List<MoneyAccount>().ToDictionary(x => x.Name, x => x);
            var exchangeRates = _repository.List<ExchangeRate>().OrderBy(x => x.Date).ToList();

            var transfers = _repository.List<RawTransaction>().Where(x => x.Type == nameof(Transfer) && !x.IsProcessed)
                .Select(x =>
                {
                    moneyAccountsByName.TryGetValue(x.Source, out var sourceFound);
                    moneyAccountsByName.TryGetValue(x.Destination, out var destinationFound);

                    var transfer = sourceFound?.TransferTo(destinationFound) ?? new Transfer();

                    transfer.Amount = x.Amount;
                    transfer.Date = x.Date;
                    transfer.Description = x.Note;

                    transfer.ExchangeRate = transfer.Type == TransferType.Exchange.ToString()
                        ? exchangeRates.Find(rt => rt.Date >= transfer.Date)
                        : null;

                    var rawTransaction = _repository.GetById<RawTransaction>(x.Id);
                    rawTransaction.IsProcessed = true;
                    _repository.Update(rawTransaction);

                    return transfer;
                });

            foreach (var transfer in transfers)
            {
                //TODO: Considerar guardar el id para referencias futuras y solo cuando se confirme que se guardo habilitar el IsProcessed
                var newTransfer = _repository.Add(transfer);
                if (newTransfer.Id == 0)
                    throw new Exception($"{JsonConvert.SerializeObject(newTransfer)}");
            }

            return View(_repository.List<Transfer>());
        }
    }
}