using Hispalance.Infraestructure.DB.IRepositories.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hispalance.Infraestructure.DB.Repositories.EF
{
    /*
     * 
     * */
    public class GenericRepositoryEF<TEntity> : GenericRepositoryEFBase<TEntity>, IGenericRepository<TEntity> 
        where TEntity : class
    {
        public GenericRepositoryEF(DbContext context) : base(context)
        {
        }

        public void Add(TEntity entityToAdd)
        {
            _context.Add(entityToAdd);
        }

        #region public methods

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            return await _context.Set<TEntity>().ToListAsync();
        }


        // SOBRECARGA 2: La completa (con params)
        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();

            // Aplicar filtro (cláusula WHERE)
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Aplicar inclusiones (eager loading)
            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }

            // Aplicar ordenamiento
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion
    }
}
