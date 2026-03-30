using MemoryOnline.Infraestructure.Generic.ConfigurationExtension;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.Generic.DBContext
{
    public class DbContextSqlServer : BaseDbContext
    {
        public DbContextSqlServer(DbContextOptions options, IConfiguration config) : base(options, config)
        {
            _connectionString = config.MyGetConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}

