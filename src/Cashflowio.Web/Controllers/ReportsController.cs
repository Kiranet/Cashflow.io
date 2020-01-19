using System;
using System.Linq;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cashflowio.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ITransactionService _transactionService;

        public ReportsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public IActionResult Calendar()
        {
            ViewData["Title"] = "Calendario";
            return View();
        }

        [HttpPost]
        public IActionResult CalendarData(DateTime start, DateTime end)
        {
            return Json(_transactionService.QueryCalendarData(start, end));
        }

        public IActionResult Analytics(int year, string type)
        {
            ViewData["Title"] = "Análisis";
            return View(_transactionService.QueryAnalyticsData(year, type));
        }

        //TODO: Reemplazar esta consulta y agregar plugin de calendario para filtrar por fecha
        public IActionResult Meals([FromServices] IRepository repository)
        {
            ViewData["Title"] = "Comida";

            repository.List<ExpenseCategory>();
            
            return View(repository.List<Expense>().Where(x => x.Destination.Name == "Comida" &&
                                                              x.Date.Year == 2019 && x.Date.Month == 12));
        }
    }
}