namespace MemoryOnline.Infraestructure.Generic.IRepositories.Generic
{
    public interface IGenericRepository<TEntity> : IGenericRepositoryRead<TEntity>, IGenericRepositoryWrite<TEntity>
        where TEntity : class //, IEntity
    {
      
    }
}
