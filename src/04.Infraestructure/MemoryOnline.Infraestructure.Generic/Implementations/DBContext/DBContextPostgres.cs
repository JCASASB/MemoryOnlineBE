using MemoryOnline.Infraestructure.Generic.ConfigurationExtension;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.Generic.DBContext
{
    public class DBContextPostgres : BaseDbContext
    {
        public DBContextPostgres(DbContextOptions options, IConfiguration config) : base(options, config)
        {
            _connectionString = config.MyGetConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //NECESITO HACER ALGO PARA QUE el addmigration funcione cuando tiene dependency injection . ahora no va,

            // replace with your Server Version and Type
            options.UseNpgsql(_connectionString);

            base.OnConfiguring(options);
        }
    }

}
