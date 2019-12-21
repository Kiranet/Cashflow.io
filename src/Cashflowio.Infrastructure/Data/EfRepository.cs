using System.Collections.Generic;
using System.Linq;
using Cashflowio.Core.Interfaces;
using Cashflowio.Core.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Cashflowio.Infrastructure.Data
{
    public class EfRepository : IRepository
    {
        private readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById<T>(int id) where T : BaseEntity
        {
            return _dbContext.Set<T>().SingleOrDefault(e => e.Id == id);
        }

        public List<T> List<T>() where T : BaseEntity
        {
            return _dbContext.Set<T>().ToList();
        }

        public List<T> ListWithNoTracking<T>() where T : BaseEntity
        {
            return _dbContext.Set<T>().AsNoTracking().ToList();
        }

        public T Add<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public int AddRange<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            _dbContext.Set<T>().AddRange(entities);
            return _dbContext.SaveChanges();
        }

        public void Delete<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Update<T>(T entity) where T : BaseEntity
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}