using MemoryOnline.Infraestructure.Generic.ConfigurationExtension;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.Generic.DBContext
{
    public class DBContextInMemory : BaseDbContext
    {
        public DBContextInMemory(DbContextOptions options, IConfiguration config) : base(options, config)
        {
            _connectionString = config.MyGetConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("nameDbRandom");

            base.OnConfiguring(options);
        }
    }

}
