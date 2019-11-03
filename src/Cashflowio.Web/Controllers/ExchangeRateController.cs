using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.EJ2.Base;

namespace Cashflowio.Web.Controllers
{
    public class ExchangeRateController : Controller
    {
        private readonly IRepository _repository;

        public ExchangeRateController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.List<ExchangeRate>());
        }

        [HttpPost]
        public ActionResult Insert([FromBody] CRUDModel model)
        {
            var exchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(model.Value.ToString());
            return Json(_repository.Add(exchangeRate));
        }
        
        public IActionResult Remove([FromBody] CRUDModel model)
        {
            var exchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(model.Value.ToString());
            
            //TODO: Me quede aqui
            
            return Json(_repository.Add(exchangeRate));
        }

        public IActionResult DataSource([FromBody] DataManagerRequest dm)
        {
            IEnumerable DataSource = _repository.List<ExchangeRate>();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search); //Search
            }

            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }

            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }

            int count = DataSource.Cast<ExchangeRate>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip); //Paging
            }

            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }

            return dm.RequiresCounts ? Json(new {result = DataSource, count = count}) : Json(DataSource);
        }

       
    }
}