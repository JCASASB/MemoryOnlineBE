using System.Linq.Expressions;

namespace Hispalance.Infraestructure.DB.IRepositories.Generic
{
    public interface IGenericRepositoryRead<TEntity> : IGenericBaseRepository<TEntity>
    {
        // Versión para consultas rápidas o sin relaciones
        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null
            , Func<IQueryable<TEntity>
                , IOrderedQueryable<TEntity>> orderBy = null
        );

        // Versión que permite incluir propiedades relacionadas usando params
        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includeProperties
        );

    }
}
