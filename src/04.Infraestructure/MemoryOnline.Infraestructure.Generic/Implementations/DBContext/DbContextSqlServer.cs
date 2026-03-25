using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.Generic.DBContext
{
    public class DbContextSqlServer : DBContextMyBase, IMyDbContext
    {
        public DbContextSqlServer(IConfiguration config) : base(config)
        {
            _connectionString = GetConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}

