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
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    // Extraer el nombre de la propiedad eliminando el nodo Convert
                    // que el compilador añade al castear a object
                    var body = include.Body;
                    if (body is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
                        body = unary.Operand;

                    if (body is MemberExpression member)
                        query = query.Include(member.Member.Name);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet; // Sin AsNoTracking para debug

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    var body = include.Body;
                    if (body is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
                        body = unary.Operand;

                    if (body is MemberExpression member)
                        query = query.Include(member.Member.Name);
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
            var context = (DbContext)_context;
            var entry = context.Entry(entityToUpdate);

            if (entry.State == EntityState.Detached)
            {
                _dbSet.Attach(entityToUpdate);
            }

            // Usar DetectChanges para que EF Core detecte nuevas entidades
            // en las propiedades de navegación (Players, Cards, etc.)
            context.ChangeTracker.DetectChanges();
            entry.State = EntityState.Modified;

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
