using Hispalance.Infraestructure.DB.IRepositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Hispalance.Infraestructure.DB.Repositories.EF
{
    public class GenericRepositoryEFRead<TEntity> : GenericRepositoryEF<TEntity>, IGenericRepositoryRead<TEntity> 
        where TEntity : class
    {
        public GenericRepositoryEFRead(DbContext context) : base(context)
        {
        }
    }
}
