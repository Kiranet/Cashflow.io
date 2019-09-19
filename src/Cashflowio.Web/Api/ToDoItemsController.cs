using System.Linq;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Web.ApiModels;
using Cashflowio.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Cashflowio.Web.Api
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class RawTransactionsController : Controller
    {
        private readonly IRepository _repository;

        public RawTransactionsController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: api/RawTransactions
        [HttpGet]
        public IActionResult List()
        {
            var items = _repository.List<RawTransaction>()
                            .Select(RawTransactionDTO.FromRawTransaction);
            return Ok(items);
        }

        // GET: api/RawTransactions
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = RawTransactionDTO.FromRawTransaction(_repository.GetById<RawTransaction>(id));
            return Ok(item);
        }

        // POST: api/RawTransactions
        [HttpPost]
        public IActionResult Post([FromBody] RawTransactionDTO item)
        {
            var RawTransaction = new RawTransaction()
            {
                //Title = item.Title,
                //Description = item.Description
            };
            _repository.Add(RawTransaction);
            return Ok(RawTransactionDTO.FromRawTransaction(RawTransaction));
        }

        [HttpPatch("{id:int}/complete")]
        public IActionResult Complete(int id)
        {
            var RawTransaction = _repository.GetById<RawTransaction>(id);
            //RawTransaction.MarkComplete();
            _repository.Update(RawTransaction);

            return Ok(RawTransactionDTO.FromRawTransaction(RawTransaction));
        }
    }
}
