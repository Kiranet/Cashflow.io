using Cashflowio.Core.Interfaces;
using Cashflowio.Core.SharedKernel;
using Cashflowio.Web.Libs.Syncfusion;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

namespace Cashflowio.Web.Controllers.Abstractions
{
    public abstract class CrudController<T> : Controller where T : BaseEntity, new()
    {
        protected readonly IRepository Repository;

        protected CrudController(IRepository repository)
        {
            Repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Crud([FromBody] CRUDModel crudModel)
        {
            var model = new T();

            switch (crudModel.Action)
            {
                case "insert":
                    model = Repository.Add(crudModel.GetModel<T>());
                    break;
                case "update":
                    model = crudModel.GetModel<T>();
                    Repository.Update(model);
                    break;
                case "remove":
                    model = Repository.GetById<T>(int.Parse(crudModel.Key.ToString()));
                    Repository.Delete(model);
                    break;
            }

            return Json(model);
        }

        public IActionResult DataSource([FromBody] DataManagerRequest dm)
        {
            return dm.FilterDataSource(Repository.List<T>());
        }
    }
}