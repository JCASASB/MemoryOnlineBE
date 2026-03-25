using Hispalance.Infraestructure.DB.IRepositories.Generic;

namespace Hispalance.Infraestructure.DB.Repositories.EF
{
    public class GenericRepositoryEFRead<TEntity> : GenericRepositoryEF<TEntity>, IGenericRepositoryRead<TEntity> 
        where TEntity : class
    {
        public GenericRepositoryEFRead(IMyDbContext context) : base(context)
        {
        }
    }
}
