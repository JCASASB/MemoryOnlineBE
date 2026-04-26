using MemoryOnline.Infraestructure.Generic.ConfigurationExtension;
using MemoryOnline.Infraestructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.Generic.DBContext
{
    public abstract class BaseDbContext : DbContext
    {
        #region properties
        protected IConfiguration _config;

        protected string _connectionString;

        protected string _databaseName;
        #endregion

        public BaseDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {   
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Usas tu método de extensión aquí para todos los hijos
                optionsBuilder.UseSqlServer(_config.MyGetConnectionString());
            }
        }
    }
}
