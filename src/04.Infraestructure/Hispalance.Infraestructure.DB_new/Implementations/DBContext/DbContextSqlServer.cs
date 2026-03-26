using Microsoft.EntityFrameworkCore;

namespace Hispalance.Infraestructure.DB.DBContext
{
    public class DbContextSqlServer : DbContext, IMyDbContext
    {
        private string _connectionString;

        public DbContextSqlServer(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}

