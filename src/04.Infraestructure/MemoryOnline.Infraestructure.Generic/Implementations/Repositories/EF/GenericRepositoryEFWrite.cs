using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
using MemoryOnline.Infraestructure.Repository;

namespace MemoryOnline.Infraestructure.Generic.Repositories.EF
{
    public class GenericRepositoryEFWrite<TEntity> : GenericRepositoryEF<TEntity>, IGenericRepositoryWrite<TEntity>
        where TEntity : class
    {
        public GenericRepositoryEFWrite(MyDbContext context) : base(context)
        {
        }
    }
}
