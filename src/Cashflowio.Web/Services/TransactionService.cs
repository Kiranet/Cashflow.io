using System;
using System.Collections.Generic;
using System.Linq;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cashflowio.Web.Services
{
    public interface ITransactionService
    {
        DashboardViewModel QueryDashboardData(int selectedYear);
        List<CalendarEventViewModel> QueryCalendarData(DateTime start, DateTime end);
        AnalyticsViewModel QueryAnalyticsData(int year, string type);
    }

    public class TransactionService : ITransactionService
    {
        private readonly IRepository _repository;

        public TransactionService(IRepository repository)
        {
            _repository = repository;

            AllConcepts = _repository.List<Concept>();
            AllMoneyAccounts = _repository.List<MoneyAccount>();
            AllIncomeSources = _repository.List<IncomeSource>();
            AllExpenseCategories = _repository.List<ExpenseCategory>();

            var income = _repository.List<Income>();
            var expenses = _repository.List<Expense>();
            var transfers = _repository.List<Transfer>();
            var creditCharges = _repository.List<CreditCharge>();
            var creditPayments = _repository.List<CreditPayment>();

            AllTransactions = income.Concat<ITransaction>(expenses).Concat(transfers)
                .Concat(creditCharges).Concat(creditPayments).ToList();
        }

        public List<ExpenseCategory> AllExpenseCategories { get; set; }

        public List<IncomeSource> AllIncomeSources { get; set; }

        public List<MoneyAccount> AllMoneyAccounts { get; set; }

        public List<Concept> AllConcepts { get; set; }

        public List<ITransaction> AllTransactions { get; set; }

        public List<int> AllYears => _repository.List<RawTransaction>()
            .Select(x => x.Date.Year).Distinct().OrderByDescending(x => x).ToList();

        public DashboardViewModel QueryDashboardData(int selectedYear)
        {
            var income = AllTransactions.Where(x => x is Income).Cast<Income>().ToList();
            var vm = new DashboardViewModel
            {
                Years = new SelectList(AllYears, selectedYear),
                Year = selectedYear
            };

            var transfers = AllTransactions.Where(x => x is Transfer).Cast<Transfer>().ToList();
            var expenses = AllTransactions.Where(x => x is Expense).Cast<Expense>().ToList();
            var creditCharges = AllTransactions.Where(x => x is CreditCharge).Cast<CreditCharge>().ToList();
            //var creditPayments = AllTransactions.Where(x => x is CreditPayment).Cast<CreditPayment>().ToList();

            if (selectedYear != 0)
            {
                income = income.Where(x => x.Date.Year == selectedYear).ToList();
                transfers = transfers.Where(x => x.Date.Year == selectedYear).ToList();
                expenses = expenses.Where(x => x.Date.Year == selectedYear).ToList();
                creditCharges = creditCharges.Where(x => x.Date.Year == selectedYear).ToList();
                //creditPayments = creditPayments.Where(x => x.Date.Year == selectedYear).ToList();
            }

            var exchangeRateById = _repository.List<ExchangeRate>().ToDictionary(x => x.Id, x => x.Value);

            foreach (var incomeSource in AllIncomeSources)
            {
                var exitIncome = income.Where(x => x.SourceId == incomeSource.Id).ToList();

                var item = new IncomeSourceViewModel
                {
                    Name = incomeSource.Name,
                    TotalAmount = exitIncome.Sum(x => x.Amount)
                };

                foreach (var group in exitIncome.GroupBy(x => x.Description))
                    item.Concepts.Add(new IncomeConceptViewModel
                    {
                        Description = group.Key,
                        Amount = group.Sum(x => x.Amount)
                    });

                vm.Income.Add(item);
            }

            foreach (var moneyAccount in AllMoneyAccounts)
            {
                var incomeReceived = income.Where(x => x.DestinationId == moneyAccount.Id);
                //var paymentReceived = creditPayments.Where(x => x.CreditCharge.SourceId == moneyAccount.Id);
                var sent = transfers.Where(x => x.SourceId == moneyAccount.Id).ToList();
                var received = transfers.Where(x => x.DestinationId == moneyAccount.Id).ToList();
                var spent = expenses.Where(x => x.SourceId == moneyAccount.Id).ToList();
                var debt = creditCharges.Where(x => x.SourceId == moneyAccount.Id);
                var balance = incomeReceived.Sum(x => x.Amount)
                              + received.Sum(x => x.Amount)
                              //              + paymentReceived.Sum(x => x.Amount)
                              - sent.Sum(x => x.Amount)
                              - debt.Sum(x => x.Amount)
                              - spent.Sum(x => x.ExchangeRate == null ? x.Amount : x.Amount * x.ExchangeRate.Value);
                var transferSentByType = sent.GroupBy(x => x.Destination.Name)
                    .OrderByDescending(g => g.Sum(i => i.Amount));
                var transferReceivedByType = received.GroupBy(x => x.Source.Name)
                    .OrderByDescending(g => g.Sum(i => i.Amount));

                var item = new MoneyAccountViewModel
                {
                    Name = moneyAccount.Name,
                    Balance = balance
                };

                foreach (var group in transferReceivedByType)
                    item.Concepts.Add(new MoneyAccountConceptViewModel
                    {
                        Icon = "fa-angle-down text-success",
                        Amount = group.Sum(x => x.Amount),
                        Count = group.Count(),
                        Name = group.Key,
                        Source = group.Key,
                        Destination = moneyAccount.Name,
                        Description = "Received"
                    });

                foreach (var group in transferSentByType)
                    item.Concepts.Add(new MoneyAccountConceptViewModel
                    {
                        Icon = "fa-angle-up text-danger",
                        Amount = group.Sum(x => x.Amount),
                        Name = group.Key,
                        Count = group.Count(),
                        Source = moneyAccount.Name,
                        Destination = group.Key,
                        Description = "Sent"
                    });

                vm.MoneyAccounts.Add(item);
            }

            foreach (var expenseCategory in AllExpenseCategories)
            {
                var debtFromCategory = creditCharges.Where(x => x.DestinationId == expenseCategory.Id).ToList();
                var expensesFromCategory = debtFromCategory
                    .Concat<MoneyOutflow>(expenses.Where(x => x.DestinationId == expenseCategory.Id)).ToList();
                var expensesByConcept = expensesFromCategory.GroupBy(x => x.Concept.Name)
                    .OrderByDescending(g =>
                        g.Sum(x => x.ExchangeRate == null ? x.Amount : x.Amount * x.ExchangeRate.Value));

                var item = new ExpenseCategoryViewModel
                {
                    Name = expenseCategory.Name,
                    Amount = expensesFromCategory.Sum(x =>
                        x.ExchangeRateId == null ? x.Amount : x.Amount * exchangeRateById[x.ExchangeRateId.Value])
                };

                foreach (var moneyOutflow in expensesByConcept)
                    item.Concepts.Add(new ExpenseConceptViewModel
                    {
                        Description = moneyOutflow.Key,
                        ExpenseCount = moneyOutflow.Count(x => x is Expense),
                        CreditCount = moneyOutflow.Count(x => x is CreditCharge),
                        Amount = moneyOutflow.Sum(x =>
                            x.ExchangeRate == null ? x.Amount : x.Amount * x.ExchangeRate.Value)
                    });

                vm.ExpenseCategories.Add(item);
            }

            return vm;
        }

        public List<CalendarEventViewModel> QueryCalendarData(DateTime start, DateTime end)
        {
            var transactions = AllTransactions.Where(x => x.Date >= start && x.Date <= end).ToList();
            var events = new List<CalendarEventViewModel>();

            foreach (var transaction in transactions)
            {
                var item = new CalendarEventViewModel
                    {Start = transaction.Date.ToString("yyyy-MM-dd"), Title = $"{transaction.Amount:C0}"};
                var subtitle = "";

                switch (transaction)
                {
                    case CreditPayment creditPayment:
                        item.Id = creditPayment.Id;
                        item.Color = "#007bff;";
                        subtitle = creditPayment.Description;
                        break;
                    case Expense expense:
                        item.Id = expense.Id;
                        item.Color = "#dc3545";
                        subtitle = expense.Concept.Name;
                        break;
                    case Income income:
                        item.Id = income.Id;
                        item.Color = "#28a745";
                        subtitle = income.Description;
                        break;
                    case CreditCharge creditCharge:
                        item.Id = creditCharge.Id;
                        item.Color = "#ffc107";
                        subtitle = creditCharge.Concept.Name;
                        break;
                    case Transfer transfer:
                        item.Id = transfer.Id;
                        item.Color = "#17a2b8";
                        subtitle = transfer.Type;
                        break;
                }

                item.Title += " " + subtitle;

                events.Add(item);
            }

            return events;
        }

        public AnalyticsViewModel QueryAnalyticsData(int year, string type)
        {
            var vm = new AnalyticsViewModel
            {
                Year = year,
                Type = type,
                Years = new SelectList(AllYears, year)
            };

            var transactions = year > 0 ? AllTransactions.Where(x => x.Date.Year == year).ToList() : AllTransactions;
            
            switch (type)
            {
                case "Income":
                    foreach (var income in transactions.Where(x => x is Income).Cast<Income>())
                    {
                        vm.Result.Add(new TransactionViewModel
                        {
                            Amount = income.Amount,
                            Date = income.Date,
                            Currency = Currency.MXN.ToString(),
                            Source = income.Source.Name,
                            Destination = income.Destination.Name,
                            Description = income.Description
                        });
                    }

                    break;
                case "Expense":
                    foreach (var expense in transactions.Where(x => x is Expense).Cast<Expense>())
                    {
                        vm.Result.Add(new TransactionViewModel
                        {
                            Amount = expense.Amount,
                            Date = expense.Date,
                            Currency = expense.Currency,
                            Source = expense.Source.Name,
                            Destination = expense.Destination.Name,
                            Description = expense.Concept.Name
                        });
                    }

                    break;
                case "CreditCharge":
                    foreach (var creditCharge in transactions.Where(x => x is CreditCharge).Cast<CreditCharge>())
                    {
                        vm.Result.Add(new TransactionViewModel
                        {
                            Amount = creditCharge.Amount,
                            Date = creditCharge.Date,
                            Currency = Currency.MXN.ToString(),
                            Source = creditCharge.Source.Name,
                            Destination = creditCharge.Destination.Name,
                            Description = creditCharge.Concept.Name
                        });
                    }

                    break;
                case "CreditPayment":
                    foreach (var creditPayment in transactions.Where(x => x is CreditPayment).Cast<CreditPayment>())
                    {
                        vm.Result.Add(new TransactionViewModel
                        {
                            Amount = creditPayment.Amount,
                            Date = creditPayment.Date,
                            Currency = Currency.MXN.ToString(),
                            Source = creditPayment.Source.Name,
                            Description = creditPayment.Description
                        });
                    }

                    break;
                case "Transfer":
                    foreach (var transfer in transactions.Where(x => x is Transfer).Cast<Transfer>())
                    {
                        vm.Result.Add(new TransactionViewModel
                        {
                            Amount = transfer.Amount,
                            Date = transfer.Date,
                            Currency = transfer.Type,
                            Source = transfer.Source.Name,
                            Destination = transfer.Destination.Name,
                            Description = transfer.Description
                        });
                    }

                    break;
            }

            return vm;
        }
    }
}