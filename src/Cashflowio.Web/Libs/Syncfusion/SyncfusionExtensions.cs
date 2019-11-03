using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.EJ2.Base;

namespace Cashflowio.Web.Libs.Syncfusion
{
    public static class SyncfusionExtensions
    {
        public static JsonResult FilterDataSource<T>(this DataManagerRequest dm, IEnumerable<T> dataSource)
        {
            var operation = new DataOperations();
            if (dm.Search != null && dm.Search.Any())
                dataSource = operation.PerformSearching(dataSource, dm.Search);
            if (dm.Sorted != null && dm.Sorted.Any())
                dataSource = operation.PerformSorting(dataSource, dm.Sorted);
            if (dm.Where != null && dm.Where.Any())
                dataSource = operation.PerformFiltering(dataSource, dm.Where, dm.Where[0].Operator);

            var dataList = dataSource.ToList();
            var count = dataList.Count;
            if (dm.Skip != 0) dataList = operation.PerformSkip(dataList, dm.Skip).ToList();
            if (dm.Take != 0) dataList = operation.PerformTake(dataList, dm.Take).ToList();
            return dm.RequiresCounts ? new JsonResult(new {result = dataList, count}) : new JsonResult(dataSource);
        }
    }

    public static class CrudModelExtensions
    {
        public static T GetModel<T>(this CRUDModel crudModel)
        {
            return JsonConvert.DeserializeObject<T>(crudModel.Value.ToString());
        }
    }
}