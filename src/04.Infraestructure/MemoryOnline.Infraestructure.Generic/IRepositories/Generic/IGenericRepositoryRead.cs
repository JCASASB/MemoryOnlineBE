using System.Linq.Expressions;

namespace MemoryOnline.Infraestructure.Generic.IRepositories.Generic
{
    public interface IGenericRepositoryRead<TEntity> : IGenericBaseRepository<TEntity>
    {
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null
            , Func<IQueryable<TEntity>
            , IOrderedQueryable<TEntity>> orderBy = null
            , string includeProperties = ""
        );

        IEnumerable<TEntity> GetAll(
              Func<IQueryable<TEntity>
            , IOrderedQueryable<TEntity>> orderBy = null
            , string includeProperties = ""
        );

        TEntity GetById(object id);
        IEnumerable<TEntity> GetPagedElements<TKey>(int pageIndex, int pageCount,
           Expression<Func<TEntity, TKey>> orderByExpression, bool ascending = true);

     

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        

    }
}
