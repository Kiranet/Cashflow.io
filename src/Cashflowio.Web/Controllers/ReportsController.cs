using System;
using System.Collections.Generic;
using System.Linq;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Web.Models;
using Cashflowio.Web.Services;
using FluentDateTime;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cashflowio.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IRepository _repository;

        public ReportsController(ITransactionService transactionService, IRepository repository)
        {
            _transactionService = transactionService;
            _repository = repository;
        }

        public IActionResult Calendar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CalendarData(DateTime start, DateTime end)
        {
            var concepts = _repository.List<Concept>();
            var transactions = _transactionService.QueryCalendarData(start, end);
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

            return Json(events, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public IActionResult Pivot()
        {
            _repository.List<ExpenseCategory>();
            _repository.List<MoneyAccount>();
            
            return View(_transactionService
                .QueryCalendarData(DateTime.Now.FirstDayOfYear(), DateTime.Now.LastDayOfYear())
                .Where(x => x is Expense).Cast<Expense>());
        }
    }
}