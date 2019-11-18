using System;
using System.Collections.Generic;
using System.Linq;
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
        protected Func<T, int, bool> FilterBy = null;

        protected CrudController(IRepository repository)
        {
            Repository = repository;
        }

        public IActionResult Index()
        {
            ViewBag.Id = GetIdFromRoute();
            return View();
        }

        public IActionResult DataSource([FromBody] DataManagerRequest dm)
        {
            IEnumerable<T> elements = Repository.List<T>();

            if (FilterBy == null) return dm.FilterDataSource(elements);

            var id = GetIdFromRoute();

            if (id > 0)
                elements = elements.Where(x => FilterBy(x, id));

            return dm.FilterDataSource(elements);
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

        [NonAction]
        private int GetIdFromRoute()
        {
            Request.RouteValues.TryGetValue("id", out var value);
            int.TryParse(value?.ToString(), out var id);
            return id;
        }
    }
}