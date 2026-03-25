using Microsoft.EntityFrameworkCore;

namespace Hispalance.Infraestructure.DB.Repositories.EF
{
    public class GenericRepositoryEFBase<TEntity> where TEntity : class
    {
        #region declarations
        protected   DbSet<TEntity> _dbSet;
        protected   IMyDbContext _context;
        #endregion

        public GenericRepositoryEFBase(IMyDbContext context)
        {
            this._dbSet = ((DbContext)context).Set<TEntity>();
            this._context = context;
        }
    }
}
