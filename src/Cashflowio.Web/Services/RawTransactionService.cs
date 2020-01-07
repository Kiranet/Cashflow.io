using System.Collections.Generic;
using System.Linq;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;

namespace Cashflowio.Web.Services
{
    public interface IRawTransactionService
    {
        List<RawTransaction> QueryAll(bool isUpdateNeeded);
    }

    public class RawTransactionService : IRawTransactionService
    {
        private readonly IRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RawTransactionService(IRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<RawTransaction> QueryAll(bool isUpdateNeeded)
        {
            var transactions = _repository.List<RawTransaction>();
            if (!isUpdateNeeded) return transactions;

            var filePath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "data", "CoinKeeper.xlsx");
            var newTransactions = RawTransactionFactory.ReadFromFile(filePath);
            var lastSavedDate = transactions.Max(x => x.Date);

            isUpdateNeeded = newTransactions.All(x => x.Date > lastSavedDate);
            if (!isUpdateNeeded) return transactions;

            _repository.AddRange(newTransactions);
            return _repository.List<RawTransaction>();
        }
    }
}