using System;
using System.Collections.Generic;
using System.Linq;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Cashflowio.Web.Controllers
{
    public class QueryController : Controller
    {
        private readonly IRepository _repository;

        public QueryController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Transfer()
        {
            foreach (var transfer in GetTransfersFromRawTransactions())
            {
                var newTransfer = _repository.Add(transfer);
                if (newTransfer.Id == 0)
                    throw new Exception($"{JsonConvert.SerializeObject(newTransfer)}");

                var rawTransaction = _repository.GetById<RawTransaction>(newTransfer.RawTransactionId);
                rawTransaction.IsProcessed = true;
                _repository.Update(rawTransaction);
            }

            return View(_repository.List<Transfer>());
        }

        private IEnumerable<Transfer> GetTransfersFromRawTransactions()
        {
            var moneyAccountsByName = _repository.List<MoneyAccount>().ToDictionary(x => x.Name);
            var exchangeRates = _repository.List<ExchangeRate>().OrderBy(x => x.Date).ToList();

            var transfers = _repository.List<RawTransaction>()
                .Where(rt => rt.Type == nameof(Transfer) && !rt.IsProcessed)
                .Select(rt =>
                {
                    moneyAccountsByName.TryGetValue(rt.Source, out var sourceFound);
                    moneyAccountsByName.TryGetValue(rt.Destination, out var destinationFound);

                    var transfer = sourceFound?.TransferTo(destinationFound) ?? new Transfer();

                    transfer.RawTransactionId = rt.Id;
                    transfer.Amount = rt.Amount;
                    transfer.Date = rt.Date;
                    transfer.Description = rt.Note;

                    transfer.ExchangeRate = transfer.Type == TransferType.Exchange.ToString()
                        ? exchangeRates.Find(x => x.Date >= transfer.Date)
                        : null;

                    return transfer;
                });
            return transfers;
        }

        private IEnumerable<Income> GetIncomeFromRawTransactions()
        {
            var incomeSourcesByName = _repository.List<IncomeSource>().ToDictionary(x => x.Name);
            var moneyAccountsByName = _repository.List<MoneyAccount>().ToDictionary(x => x.Name);

            var incomeFromSource = _repository.List<IncomeSource>()
                .Select(x => x.GenerateIncome()).SelectMany(x => x).ToList();

            var income = _repository.List<RawTransaction>()
                .Where(rt => rt.Type == nameof(Income) && !rt.IsProcessed)
                .Select(rt =>
                {
                    var newIncome = new Income
                    {
                        RawTransactionId = rt.Id
                    };

                    var incomeFound =
                        incomeFromSource.FirstOrDefault(x => x.Amount.Equals(rt.Amount) && x.Date.Date == rt.Date.Date);

                    if (incomeFound != null)
                    {
                        newIncome.Date = incomeFound.Date;
                        newIncome.Amount = incomeFound.Amount;
                        newIncome.SourceId = incomeFound.SourceId;
                        newIncome.DestinationId = incomeFound.DestinationId;
                        newIncome.Description = incomeFound.Description;
                    }
                    else
                    {
                        newIncome.Date = rt.Date;
                        newIncome.Amount = rt.Amount;
                        incomeSourcesByName.TryGetValue(rt.Source, out var sourceFound);
                        moneyAccountsByName.TryGetValue(rt.Destination, out var destinationFound);
                        newIncome.SourceId = sourceFound?.Id ?? 0;
                        newIncome.DestinationId = destinationFound?.Id ?? 0;
                        newIncome.Description = $"{rt.Tag} {rt.Note}";
                    }

                    return newIncome;
                });

            return income;
        }

        public IActionResult Income()
        {
            foreach (var income in GetIncomeFromRawTransactions())
            {
                var newIncome = _repository.Add(income);
                if (newIncome.Id == 0)
                    throw new Exception($"{JsonConvert.SerializeObject(newIncome)}");

                var rawTransaction = _repository.GetById<RawTransaction>(newIncome.RawTransactionId);
                rawTransaction.IsProcessed = true;
                _repository.Update(rawTransaction);
            }

            return View(_repository.List<Income>());
        }

        public IActionResult Expense()
        {
            var expensesNotProcessed = _repository.List<RawTransaction>()
                .Where(rt => rt.Type == nameof(Expense) && !rt.IsProcessed).ToList();

            var newExpenseCategories = expensesNotProcessed
                .Select(x => x.Destination).Distinct()
                .Select(x => new ExpenseCategory {Name = x});

            var expenseCategories = _repository.List<ExpenseCategory>().ToList();

            foreach (var expenseCategory in newExpenseCategories)
            {
                if (expenseCategories.Find(x => x.Name == expenseCategory.Name) == null)
                    _repository.Add(expenseCategory);
            }
            
            var newConcepts = expensesNotProcessed
                .Select(x => x.Tag).Distinct()
                .Select(x => new Concept {Name = x});

            var concepts = _repository.List<Concept>().ToList();

            foreach (var concept in newConcepts)
            {
                if (concepts.Find(x => x.Name == concept.Name) == null)
                    _repository.Add(concept);
            }

            return View(_repository.List<Concept>());
        }
    }
}