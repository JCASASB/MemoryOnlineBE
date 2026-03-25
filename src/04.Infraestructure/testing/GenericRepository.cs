using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace testing
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly MyDbContext _empDBContext;
        public Repository(MyDbContext empDBContext)
        {
            _empDBContext = empDBContext;
        }
        public void Add(TEntity entity)
        {
            _empDBContext.Set<TEntity>().Add(entity);
            _empDBContext.SaveChanges();
        }
        public void AddMany(IEnumerable<TEntity> entities)
        {
            _empDBContext.Set<TEntity>().AddRange(entities);
            _empDBContext.SaveChanges();
        }
        public void Delete(TEntity entity)  
        {
            _empDBContext.Set<TEntity>().Remove(entity);
            _empDBContext.SaveChanges();
        }
        public void DeleteMany(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = Find(predicate);
            _empDBContext.Set<TEntity>().RemoveRange(entities);
            _empDBContext.SaveChanges();
        }
        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null)
        {
            return Get(findOptions).FirstOrDefault(predicate)!;
        }
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null)
        {
            return Get(findOptions).Where(predicate);
        }
        public IQueryable<TEntity> GetAll(FindOptions? findOptions = null)
        {
            return Get(findOptions);
        }
        public void Update(TEntity entity)
        {
            _empDBContext.Set<TEntity>().Update(entity);
            _empDBContext.SaveChanges();
        }
        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _empDBContext.Set<TEntity>().Any(predicate);
        }
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _empDBContext.Set<TEntity>().Count(predicate);
        }
        private DbSet<TEntity> Get(FindOptions? findOptions = null)
        {
            findOptions ??= new FindOptions();
            var entity = _empDBContext.Set<TEntity>();
            if (findOptions.IsAsNoTracking && findOptions.IsIgnoreAutoIncludes)
            {
                entity.IgnoreAutoIncludes().AsNoTracking();
            }
            else if (findOptions.IsIgnoreAutoIncludes)
            {
                entity.IgnoreAutoIncludes();
            }
            else if (findOptions.IsAsNoTracking)
            {
                entity.AsNoTracking();
            }
            return entity;
        }
    }
}
