using Microsoft.EntityFrameworkCore;

namespace Hispalance.Infraestructure.DB.Repositories.EF
{
    public class GenericRepositoryEFBase<TEntity> where TEntity : class
    {
        #region declarations
        protected readonly DbContext _context;
        #endregion

        public GenericRepositoryEFBase(DbContext context)
        {
            this._context = context;
        }
    }
}
