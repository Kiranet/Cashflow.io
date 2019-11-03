using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Web.Libs.Syncfusion;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        public IActionResult DataSource([FromBody] DataManagerRequest dm)
        {
            return dm.FilterDataSource(_repository.List<ExchangeRate>());
        }

        [HttpPost]
        public ActionResult Crud([FromBody] CRUDModel crudModel)
        {
            var model = new ExchangeRate();

            switch (crudModel.Action)
            {
                case "insert":
                    model = _repository.Add(crudModel.GetModel<ExchangeRate>());
                    break;
                case "update":
                    model = crudModel.GetModel<ExchangeRate>();
                    _repository.Update(model);
                    break;
                case "remove":
                    model = _repository.GetById<ExchangeRate>(int.Parse(crudModel.Key.ToString()));
                    _repository.Delete(model);
                    break;
            }

            return Json(model);
        }
    }
}