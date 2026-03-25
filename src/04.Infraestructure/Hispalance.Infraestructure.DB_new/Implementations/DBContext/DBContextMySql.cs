using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hispalance.Infraestructure.DB.DBContext
{
    public class DBContextMySql : DBContextMyBase, IMyDbContext
    {
        public DBContextMySql(IConfiguration config) : base(config)
        {
            _connectionString = GetConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 28));

            options.UseMySql(_connectionString, serverVersion);

            base.OnConfiguring(options);
        }
     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }

}
