using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hispalance.Infraestructure.DB.DBContext
{
    public class DBContextInMemory : DBContextMyBase
    {
        public DBContextInMemory(IConfiguration config) : base(config)
        {
            _connectionString = GetConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase(_connectionString);

            base.OnConfiguring(options);
        }

        protected override string GetConnectionString()
        {
            try
            {
                var database = _config.GetSection("DBSection:Database").Value;

                return database;
            }
            catch (Exception ex)
            {
                throw new Exception("Algo falla al recuperar los datos " +
                    "de la conection string en el dbcontext", ex);
            }
        }
    }

}
