using MemoryOnline.Infraestructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.Generic.Repositories.EF
{
    public class GenericRepositoryEFBase<TEntity> where TEntity : class
    {
        #region declarations
        protected DbSet<TEntity> _dbSet;
        protected IMyDbContext _context;
        #endregion

        public GenericRepositoryEFBase(IMyDbContext context)
        {
            this._dbSet = ((DbContext)context).Set<TEntity>();
            this._context = context;
        }
    }
}
