using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
using MemoryOnline.Infraestructure.Repository;

namespace MemoryOnline.Infraestructure.Generic.Repositories.EF
{
    public class GenericRepositoryEFWrite<TEntity> : GenericRepositoryEF<TEntity>, IGenericRepositoryWrite<TEntity>
        where TEntity : class
    {
        public GenericRepositoryEFWrite(IMyDbContext context) : base(context)
        {
        }

        // Todos los métodos async heredados de GenericRepositoryEF<TEntity>:
        // - AddAsync(TEntity entityToAdd)
        // - UpdateAsync(TEntity entityToUpdate)
        // - DeleteAsync(TEntity entityToDelete)
        // - DeleteByIdAsync(object id)
        // - SaveChangesAsync()
    }
}
