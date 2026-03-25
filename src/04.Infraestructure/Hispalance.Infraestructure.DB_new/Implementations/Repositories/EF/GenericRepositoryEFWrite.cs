using Hispalance.Infraestructure.DB.IRepositories.Generic;

namespace Hispalance.Infraestructure.DB.Repositories.EF
{
    public class GenericRepositoryEFWrite<TEntity> : GenericRepositoryEF<TEntity>, IGenericRepositoryWrite<TEntity>
        where TEntity : class
    {
        public GenericRepositoryEFWrite(IMyDbContext context) : base(context)
        {
        }
    }
}
