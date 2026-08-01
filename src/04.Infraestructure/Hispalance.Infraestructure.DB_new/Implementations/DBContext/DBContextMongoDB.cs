using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Hispalance.Infraestructure.DB.DBContext
{
    public class DBContextMongoDB : DBContextMyBase
    {
        private string _database;

        public DBContextMongoDB(DbContextOptions options, IConfiguration config) : base(options, config)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMongoDB(_connectionString, _database);

            base.OnConfiguring(options);
        }
        protected override string GetConnectionString()
        {
            try
            {
                var connectionString = _config.GetSection("DBSection:MongoConnectionString").Value
                    ?? throw new Exception("DBSection:MongoConnectionString no está configurada.");

                var mongoUrl = new MongoUrl(connectionString);
                _database = mongoUrl.DatabaseName
                    ?? throw new Exception("La MongoConnectionString debe incluir el nombre de la base de datos (ej: mongodb://host:27017/memoryDB).");

                return connectionString;
            }
            catch (Exception ex)
            {
                throw new Exception("Algo falla al recuperar los datos " +
                    "de la conection string en el dbcontext", ex);
            }
        }
        protected string GetConnectionString_deprecated()
        {
            try
            {
                var server = _config.GetSection("DBSection:Server").Value;
                var port = _config.GetSection("DBSection:Port").Value;
                var database = _config.GetSection("DBSection:Database").Value;
                var user = _config.GetSection("DBSection:User").Value;
                var pass = _config.GetSection("DBSection:Password").Value;
               
                var connectionString = $"mongodb://{user}:{pass}@{server}:{port}";

                _database = database;

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
