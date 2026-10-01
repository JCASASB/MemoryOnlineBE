namespace MemoryOnline.Infraestructure.Generic.IRepositories.Generic
{
    public interface IGenericRepositoryWrite<TEntity> 
    {
        Task DeleteAsync(TEntity entityToDelete);

        Task DeleteByIdAsync(object id);

        Task<TEntity> AddAsync(TEntity entityToAdd);

        Task<TEntity> UpdateAsync(TEntity entityToUpdate);

        Task<int> SaveChangesAsync();
    }
}
