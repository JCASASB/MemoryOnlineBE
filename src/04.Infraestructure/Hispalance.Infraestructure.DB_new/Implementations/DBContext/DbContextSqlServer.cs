using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hispalance.Infraestructure.DB.DBContext
{
    public class DBContextSqlServer : DBContextMyBase
    {
        public DBContextSqlServer(IConfiguration config) : base(config)
        {
            _connectionString = GetConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //NECESITO HACER ALGO PARA QUE el addmigration funcione cuando tiene dependency injection . ahora no va,

            optionsBuilder.UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override string GetConnectionString()
        {
            try
            {
                var server = _config.GetSection("DBSection:Server").Value;
                var port = _config.GetSection("DBSection:Port").Value;
                var database = _config.GetSection("DBSection:Database").Value;
                var user = _config.GetSection("DBSection:User").Value;
                var pass = _config.GetSection("DBSection:Password").Value;
                var connectionString = String.Format("Server={0},{1};Database={2};User Id={3};Password={4};TrustServerCertificate=True;",
                    server, port, database, user, pass);

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

