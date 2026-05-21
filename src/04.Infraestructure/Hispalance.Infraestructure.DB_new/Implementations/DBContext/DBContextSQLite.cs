using Hispalance.Infraestructure.DB.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hispalance.Infraestructure.DB.Implementations.DBContext
{
    public class DBContextSQLite : DBContextMyBase
    {
        public DBContextSQLite(DbContextOptions options, IConfiguration config) : base(options, config)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //NECESITO HACER ALGO PARA QUE el addmigration funcione cuando tiene dependency injection . ahora no va,

            optionsBuilder.UseSqlite(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override string GetConnectionString()
        {
            try
            {   
                var database = _config.GetSection("DBSection:Database").Value;
                var server = _config.GetSection("DBSection:Server").Value;
                var connectionString = String.Format("Data Source={0}\\{1}.db;", server, database);

                return connectionString;
            }
            catch (Exception ex)
            {
                throw new Exception("Algo falla al recuperar los datos " +
                    "de la conection string en el dbcontext", ex);
            }
        }
    }
}