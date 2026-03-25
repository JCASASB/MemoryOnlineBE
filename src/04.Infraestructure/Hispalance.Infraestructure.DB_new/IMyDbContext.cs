using System;

namespace Hispalance.Infraestructure.DB
{
    public interface IMyDbContext : IDisposable
    {
        //DbSet<T> Set<T>() where T : class;
        int SaveChanges();

      ////  DbSet<TEntity> Set<TEntity>() where TEntity : class;
      //  DbSet Set(Type entityType);
      //  IEnumerable<DbEntityValidationResult> GetValidationErrors();
      //  DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
      //  DbEntityEntry Entry(object entity);
     //   string ConnectionString { get; set; }
     //   bool AutoDetectChangedEnabled { get; set; }
      //  void ExecuteSqlCommand(string p, params object[] o);
    }
}
