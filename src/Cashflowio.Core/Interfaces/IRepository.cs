using System.Collections.Generic;
using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Interfaces
{
    public interface IRepository
    {
        T GetById<T>(int id) where T : BaseEntity;
        List<T> List<T>() where T : BaseEntity;
        List<T> ListWithNoTracking<T>() where T : BaseEntity;
        T Add<T>(T entity) where T : BaseEntity;
        int AddRange<T>(IEnumerable<T> entities) where T : BaseEntity;
        void Update<T>(T entity) where T : BaseEntity;
        void Delete<T>(T entity) where T : BaseEntity;
    }
}