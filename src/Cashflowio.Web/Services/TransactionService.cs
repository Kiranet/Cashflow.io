using System.Linq;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cashflowio.Web.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository _repository;

        public TransactionService(IRepository repository)
        {
            _repository = repository;
        }

        public DashboardViewModel Query(int selectedYear)
        {
            var income = _repository.List<Income>().ToList();
            var expenses = _repository.List<Expense>().ToList();
            var transfers = _repository.List<Transfer>().ToList();
            var creditCharges = _repository.List<CreditCharge>().ToList();
            var creditPayments = _repository.List<CreditPayment>().ToList();

            var vm = new DashboardViewModel
            {
                Years = new SelectList(income.Select(x => x.Date.Year).Distinct(), selectedValue: selectedYear),
                SelectedYear = selectedYear
            };

            if (selectedYear != 0)
            {
                income = income.Where(x => x.Date.Year == selectedYear).ToList();
                transfers = transfers.Where(x => x.Date.Year == selectedYear).ToList();
                expenses = expenses.Where(x => x.Date.Year == selectedYear).ToList();
            }

            var moneyAccounts = _repository.List<MoneyAccount>().ToList();
            var concepts = _repository.List<Concept>().ToList();
            var incomeSources = _repository.List<IncomeSource>().ToList();
            var expenseCategories = _repository.List<ExpenseCategory>().ToList();
            var exchangeRateById = _repository.List<ExchangeRate>().ToDictionary(x => x.Id, x => x.Value);

            foreach (var incomeSource in incomeSources)
            {
                var exitIncome = income.Where(x => x.SourceId == incomeSource.Id).ToList();

                var item = new IncomeSourceViewModel
                {
                    Name = incomeSource.Name,
                    TotalAmount = exitIncome.Sum(x => x.Amount)
                };

                foreach (var group in exitIncome.GroupBy(x => x.Description))
                {
                    item.Concepts.Add(new IncomeConceptViewModel
                    {
                        Description = group.Key,
                        Amount = group.Sum(x => x.Amount)
                    });
                }

                vm.Income.Add(item);
            }

            foreach (var moneyAccount in moneyAccounts)
            {
                var incomeReceived = income.Where(x => x.DestinationId == moneyAccount.Id);
                var paymentReceived = creditPayments.Where(x => x.DestinationId == moneyAccount.Id);
                var sent = transfers.Where(x => x.SourceId == moneyAccount.Id).ToList();
                var received = transfers.Where(x => x.DestinationId == moneyAccount.Id).ToList();
                var spent = expenses.Where(x => x.SourceId == moneyAccount.Id).ToList();
                var debt = creditCharges.Where(x => x.SourceId == moneyAccount.Id);
                var balance = incomeReceived.Sum(x => x.Amount) - sent.Sum(x => x.Amount) + received.Sum(x => x.Amount)
                                                                                          + paymentReceived.Sum(x =>
                                                                                              x.Amount) - debt.Sum(x =>
                                                                                                            x.Amount)
                                                                                                        - spent.Sum(x =>
                                                                                                            x.ExchangeRate ==
                                                                                                            null
                                                                                                                ? x
                                                                                                                    .Amount
                                                                                                                : x.Amount *
                                                                                                                  x.ExchangeRate
                                                                                                                      .Value);
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
                {
                    item.Concepts.Add(new MoneyAccountConceptViewModel
                    {
                        Icon = "fa-angle-down text-success",
                        Amount = group.Sum(x => x.Amount),
                        Name = group.Key,
                        Source = group.Key,
                        Destination = moneyAccount.Name,
                        Description = "Received"
                    });
                }

                foreach (var group in transferSentByType)
                {
                    item.Concepts.Add(new MoneyAccountConceptViewModel
                    {
                        Icon = "fa-angle-up text-danger",
                        Amount = group.Sum(x => x.Amount),
                        Name = group.Key,
                        Source = moneyAccount.Name,
                        Destination = group.Key,
                        Description = "Sent"
                    });
                }

                vm.MoneyAccounts.Add(item);
            }

            foreach (var expenseCategory in expenseCategories)
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
                {
                    item.Concepts.Add(new ExpenseConceptViewModel
                    {
                        Description = moneyOutflow.Key,
                        Amount = moneyOutflow.Sum(x =>
                            x.ExchangeRate == null ? x.Amount : x.Amount * x.ExchangeRate.Value)
                    });
                }

                vm.ExpenseCategories.Add(item);
            }

            return vm;
        }
    }

    public interface ITransactionService
    {
        DashboardViewModel Query(int selectedYear);
    }
}