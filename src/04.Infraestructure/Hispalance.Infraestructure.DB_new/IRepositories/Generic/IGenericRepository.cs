namespace Hispalance.Infraestructure.DB.IRepositories.Generic
{
    public interface IGenericRepository<TEntity> : IGenericRepositoryRead<TEntity>, IGenericRepositoryWrite<TEntity>
        where TEntity : class //, IEntity
    {
      
    }
}
