using System.Linq;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;

namespace Cashflowio.Core
{
    public static class DatabasePopulator
    {
        public static int PopulateDatabase(IRepository repository)
        {
            if (repository.List<RawTransaction>().Any()) return 0;

            repository.Add(new RawTransaction
            {
            });

            return repository.List<RawTransaction>().Count;
        }
    }
}
