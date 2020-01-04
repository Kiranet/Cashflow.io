using System;
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
            ViewData["Title"] = nameof(Calendar);
            return View();
        }

        [HttpPost]
        public IActionResult CalendarData(DateTime start, DateTime end)
        {
            return Json(_transactionService.QueryCalendarData(start, end));
        }

        public IActionResult Analytics(int year, string type)
        {
            ViewData["Title"] = nameof(Analytics);
            return View(_transactionService.QueryAnalyticsData(year, type));
        }
    }
}