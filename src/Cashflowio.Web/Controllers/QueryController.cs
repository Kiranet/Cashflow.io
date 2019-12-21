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

        public IActionResult Transfer(int year)
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

            var transfers = _repository.List<Transfer>();

            if (year != 0)
                transfers = transfers.Where(x => x.Date.Year == year).ToList();

            return View(transfers);
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

        public IActionResult Income(int year)
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

            var incomes = _repository.List<Income>();

            if (year != 0)
                incomes = incomes.Where(x => x.Date.Year == year).ToList();

            return View(incomes);
        }

        public IActionResult Expense(int year)
        {
            foreach (var expense in GetExpenseFromRawTransactions())
            {
                var newExpense = _repository.Add(expense);
                if (newExpense.Id == 0)
                    throw new Exception($"{JsonConvert.SerializeObject(newExpense)}");

                var rawTransaction = _repository.GetById<RawTransaction>(newExpense.RawTransactionId);
                rawTransaction.IsProcessed = true;
                _repository.Update(rawTransaction);
            }

            var expenses = _repository.List<Expense>();

            if (year != 0)
                expenses = expenses.Where(x => x.Date.Year == year).ToList();

            return View(expenses);
        }

        private IEnumerable<Expense> GetExpenseFromRawTransactions()
        {
            var rawTransactions = _repository.List<RawTransaction>()
                .Where(rt => rt.Type == nameof(Expense) && !rt.IsProcessed).ToList();

            UpdateExpenseCategoriesAndConcepts(rawTransactions);

            var moneyAccountsByName = _repository.List<MoneyAccount>().ToDictionary(x => x.Name);
            var expenseCategoriesByName = _repository.List<ExpenseCategory>().ToDictionary(x => x.Name);
            var conceptsByName = _repository.List<Concept>().ToDictionary(x => x.Name);
            var exchangeRates = _repository.List<ExchangeRate>().OrderBy(x => x.Date).ToList();

            var expenses = rawTransactions
                .Select(rt =>
                {
                    moneyAccountsByName.TryGetValue(rt.Source, out var sourceFound);
                    expenseCategoriesByName.TryGetValue(rt.Destination, out var destinationFound);
                    conceptsByName.TryGetValue(rt.Tag, out var conceptFound);

                    var expense = new Expense
                    {
                        RawTransactionId = rt.Id,
                        Amount = rt.Amount,
                        Date = rt.Date,
                        Description = rt.Note,
                        Currency = rt.Currency
                    };


                    expense.ExchangeRate = rt.Currency == Currency.USD.ToString()
                        ? exchangeRates.Find(x => x.Date >= expense.Date)
                        : null;

                    expense.SourceId = sourceFound?.Id ?? 0;
                    expense.DestinationId = destinationFound?.Id ?? 0;
                    expense.ConceptId = conceptFound?.Id ?? 0;

                    return expense;
                });

            return expenses;
        }

        private void UpdateExpenseCategoriesAndConcepts(List<RawTransaction> transactions)
        {
            var newExpenseCategories = transactions
                .Select(x => x.Destination).Distinct()
                .Select(x => new ExpenseCategory {Name = x});

            var expenseCategories = _repository.List<ExpenseCategory>().ToList();

            foreach (var expenseCategory in newExpenseCategories)
            {
                if (expenseCategories.Find(x => x.Name == expenseCategory.Name) == null)
                    _repository.Add(expenseCategory);
            }

            var newConcepts = transactions
                .Select(x => x.Tag).Distinct()
                .Select(x => new Concept {Name = x});

            var concepts = _repository.List<Concept>().ToList();

            foreach (var concept in newConcepts)
            {
                if (concepts.Find(x => x.Name == concept.Name) == null)
                    _repository.Add(concept);
            }
        }

        public IActionResult Credit(int id, int year)
        {
            foreach (var creditCharge in GetCreditChargesFromRawTransactions())
            {
                var newCreditCharge = _repository.Add(creditCharge);
                if (newCreditCharge.Id == 0)
                    throw new Exception($"{JsonConvert.SerializeObject(newCreditCharge)}");

                var rawTransaction = _repository.GetById<RawTransaction>(newCreditCharge.RawTransactionId);
                rawTransaction.IsProcessed = true;
                _repository.Update(rawTransaction);
            }

            var creditCharges = _repository.List<CreditCharge>();
            
            if (year != 0)
                creditCharges = creditCharges.Where(x => x.Date.Year == year).ToList();

            return View(creditCharges);
        }

        private IEnumerable<CreditCharge> GetCreditChargesFromRawTransactions()
        {
            var rawTransactions = _repository.List<RawTransaction>()
                .Where(rt => rt.Type == nameof(Credit) && !rt.IsProcessed).ToList();

            UpdateExpenseCategoriesAndConcepts(rawTransactions);

            var moneyAccountsByName = _repository.List<MoneyAccount>().ToDictionary(x => x.Name);
            var expenseCategoriesByName = _repository.List<ExpenseCategory>().ToDictionary(x => x.Name);
            var conceptsByName = _repository.List<Concept>().ToDictionary(x => x.Name);

            return rawTransactions.Select(x =>
            {
                moneyAccountsByName.TryGetValue(x.Source, out var sourceFound);
                expenseCategoriesByName.TryGetValue(x.Destination, out var destinationFound);
                conceptsByName.TryGetValue(x.Tag, out var conceptFound);

                return new CreditCharge
                {
                    RawTransactionId = x.Id,
                    Date = x.Date,
                    Amount = x.Amount,
                    Description = x.Note,
                    SourceId = sourceFound?.Id ?? 0,
                    DestinationId = destinationFound?.Id ?? 0,
                    ConceptId = conceptFound?.Id ?? 0
                };
            });
        }

        public IActionResult Payment(int id, int year)
        {
            foreach (var creditPayment in GetCreditPaymentsFromRawTransactions())
            {
                creditPayment.Id = 0;
                var newCreditPayment = _repository.Add(creditPayment);
                if (newCreditPayment.Id == 0)
                    throw new Exception($"{JsonConvert.SerializeObject(newCreditPayment)}");

                var rawTransaction = _repository.GetById<RawTransaction>(newCreditPayment.RawTransactionId);
                rawTransaction.IsProcessed = true;
                _repository.Update(rawTransaction);
            }

            var creditPayments = _repository.List<CreditPayment>();

            if (id != 0)
                creditPayments = creditPayments.Where(x => x.CreditChargeId == id).ToList();

            if (year != 0)
                creditPayments = creditPayments.Where(x => x.Date.Year == year).ToList();

            return View(creditPayments);
        }

        private IEnumerable<CreditPayment> GetCreditPaymentsFromRawTransactions()
        {
            var rawTransactions = _repository.List<RawTransaction>()
                .Where(rt => rt.Type == nameof(Payment) && !rt.IsProcessed).ToList();

            var expenseCategory = _repository.List<ExpenseCategory>().ToList();
            var creditCharges = _repository.ListWithNoTracking<CreditCharge>().OrderByDescending(x => x.Date).ToList();
            var conceptsNamesById = _repository.List<Concept>().ToDictionary(x => x.Id, x => x.Name);
            var creditPayments = new List<CreditPayment>();

            var moneyAccountsByName = _repository.List<MoneyAccount>().ToDictionary(x => x.Name);
            var destinationNamesById = _repository.List<ExpenseCategory>().ToDictionary(x => x.Id, x => x.Name);

            foreach (var rt in rawTransactions)
            {
                moneyAccountsByName.TryGetValue(rt.Source, out var sourceFound);
                moneyAccountsByName.TryGetValue(rt.Destination, out var destinationFound);

                var containsExpenseCategory = expenseCategory.Any(x => rt.Note.Contains(x.Name));

                var creditCharge = new CreditCharge();

                if (containsExpenseCategory)
                {
                    creditCharge = creditCharges.Find(x =>
                        !x.IsPaid
                        && rt.Amount <= x.Amount
                        && rt.Date >= x.Date
                        && conceptsNamesById[x.ConceptId] == rt.Tag
                        && rt.Note.Contains(destinationNamesById[x.DestinationId]));
                }
                else
                {
                    creditCharge = creditCharges.Find(x =>
                        !x.IsPaid
                        && rt.Amount <= x.Amount
                        && rt.Date >= x.Date
                        && conceptsNamesById[x.ConceptId] == rt.Tag);
                }

                var creditPayment = new CreditPayment
                {
                    RawTransactionId = rt.Id,
                    Date = rt.Date,
                    Amount = rt.Amount,
                    Description = rt.Note,
                    SourceId = sourceFound?.Id ?? 0,
                    DestinationId = destinationFound?.Id ?? 0,
                    CreditChargeId = creditCharge?.Id
                };

                var indexOf = creditCharges.IndexOf(creditCharge);

                if (indexOf > 0)
                    creditCharges[indexOf].Payments.Add(creditPayment);

                creditPayments.Add(creditPayment);
            }

            return creditPayments;
        }
    }
}