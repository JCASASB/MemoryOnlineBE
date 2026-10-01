using Hispalance.Infraestructure.DB.IRepositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Hispalance.Infraestructure.DB.Repositories.EF
{
    public class GenericRepositoryEFWrite<TEntity> : GenericRepositoryEF<TEntity>, IGenericRepositoryWrite<TEntity>
        where TEntity : class
    {
        public GenericRepositoryEFWrite(DbContext context) : base(context)
        {
        }
    }
}
