using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hispalance.Infraestructure.DB.DBContext
{
    public class DBContextInMemory : DBContextMyBase
    {
        public DBContextInMemory(IConfiguration config) : base(config)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //NECESITO HACER ALGO PARA QUE el addmigration funcione cuando tiene dependency injection . ahora no va,

            // replace with your Server Version and Type
            options.UseInMemoryDatabase("TuCadenaDeConexion");

            base.OnConfiguring(options);
        }
    }

}
