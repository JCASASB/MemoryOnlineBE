
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
using MemoryOnline.Infraestructure.Repository;

namespace MemoryOnline.Infraestructure.Generic.Repositories.EF
{
    public class GenericRepositoryEFRead<TEntity> : GenericRepositoryEF<TEntity>, IGenericRepositoryRead<TEntity> 
        where TEntity : class
    {
        public GenericRepositoryEFRead(MyDbContext context) : base(context)
        {
        }
    }
}
