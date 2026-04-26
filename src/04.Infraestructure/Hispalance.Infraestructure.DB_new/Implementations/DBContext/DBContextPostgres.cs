using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hispalance.Infraestructure.DB.DBContext
{
    public class DBContextPostgres : DBContextMyBase
    {
        public DBContextPostgres(IConfiguration config) : base(config)
        {
            _connectionString = GetConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //NECESITO HACER ALGO PARA QUE el addmigration funcione cuando tiene dependency injection . ahora no va,

            // replace with your Server Version and Type
            options.UseNpgsql(_connectionString);

            base.OnConfiguring(options);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Referentials
            //        modelBuilder.ApplyConfiguration(new UserResultConfiguration());
        }


    }

}
