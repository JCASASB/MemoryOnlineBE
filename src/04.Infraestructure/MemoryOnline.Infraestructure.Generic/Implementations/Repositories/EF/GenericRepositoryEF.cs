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
        public GenericRepositoryEF(IMyDbContext context) : base(context)
        {
        }

        #region Read Methods

        public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        #endregion

        #region Write Methods

        public async Task<TEntity> AddAsync(TEntity entityToAdd)
        {
            await _dbSet.AddAsync(entityToAdd);
            await SaveChangesAsync();
            return entityToAdd;
        }

        public async Task<TEntity> UpdateAsync(TEntity entityToUpdate)
        {
            var existingEntity = _dbSet.Local.FirstOrDefault(e => e == entityToUpdate);

            if (existingEntity == null)
            {
                _dbSet.Attach(entityToUpdate);
            }

            ((DbContext)_context).Entry(entityToUpdate).State = EntityState.Modified;
            await SaveChangesAsync();
            return entityToUpdate;
        }

        public async Task DeleteAsync(TEntity entityToDelete)
        {
            var existingEntity = _dbSet.Local.FirstOrDefault(e => e == entityToDelete);

            if (existingEntity != null)
            {
                _dbSet.Remove(existingEntity);
            }
            else
            {
                _dbSet.Attach(entityToDelete);
                _dbSet.Remove(entityToDelete);
            }

            await SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(object id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                await DeleteAsync(entityToDelete);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await ((DbContext)_context).SaveChangesAsync();
            }
            finally
            {
                ((DbContext)_context).ChangeTracker.Clear();
            }
        }

        #endregion
    }
}
