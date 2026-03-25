using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
using MemoryOnline.Infraestructure.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MemoryOnline.Infraestructure.Generic.Repositories.EF
{
    /*
     * Cada unit of work tiene una instancia de context injectada por DI, esta la inyecto a los repositorios de cada unit of work de forma manual. Mirar las implementaciones 
     * de los constructores de unit of work
     * 
     * */
    public class GenericRepositoryEF<TEntity> : GenericRepositoryEFBase<TEntity>, IGenericRepository<TEntity> 
        where TEntity : class
    {
        public GenericRepositoryEF(MyDbContext context) : base(context)
        {
        }

        #region public methods

        public virtual IEnumerable<TEntity> GetAll(
              Func<IQueryable<TEntity>
            , IOrderedQueryable<TEntity>> orderBy = null
            , string includeProperties = ""
            )
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public virtual IEnumerable<TEntity> Get(
              Expression<Func<TEntity, bool>> filter = null
            , Func<IQueryable<TEntity>
            , IOrderedQueryable<TEntity>> orderBy = null
            , string includeProperties = ""
            )
        {
            IQueryable<TEntity> query = _dbSet;


            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }


            if (orderBy != null)
            {
                query = orderBy(query);
            }
            else
            {
            }

            return query;
        }

        public TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            //  AsNoTracking para que pueda hacer update sin error
            return _dbSet.AsNoTracking().Where(predicate);
        }

       
        public void Delete(TEntity entityToDelete)
        {
            _dbSet.Attach(entityToDelete);
            _dbSet.Remove(entityToDelete);
        }

        public void DeleteById(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Add(TEntity entityToAdd)
        {
            _dbSet.Add(entityToAdd);
        }

        public void Attach(TEntity entityToAttach)
        {
            _dbSet.Attach(entityToAttach);
        }
        public void Dettach(TEntity entityToAttach)
        {
            ((DbContext)_context).Entry<TEntity>(entityToAttach).State = EntityState.Detached;
        }

        public void Update(TEntity entityToUpdate)
        {
            ((DbContext)_context).Entry<TEntity>(entityToUpdate).State = EntityState.Modified;
        }

        public IEnumerable<TEntity> GetPagedElements<TKey>(
              int pageIndex
            , int pageCount
            , Expression<Func<TEntity, TKey>> orderByExpression
            , bool ascending = true
            )
        {
            if (pageIndex < 1)
                pageIndex = 1;

            if (orderByExpression == (Expression<Func<TEntity, TKey>>)null)
                throw new ArgumentNullException();

            return (ascending)
                ?
                   _dbSet.OrderBy(orderByExpression)
                   .Skip((pageIndex - 1) * pageCount)
                   .Take(pageCount)
                   .ToList()
                :
                    _dbSet.OrderByDescending(orderByExpression)
                    .Skip((pageIndex - 1) * pageCount)
                    .Take(pageCount)
                    .ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        #endregion
    }
}
