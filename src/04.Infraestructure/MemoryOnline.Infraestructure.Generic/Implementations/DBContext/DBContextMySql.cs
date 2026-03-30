using MemoryOnline.Infraestructure.Generic.ConfigurationExtension;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.Generic.DBContext
{

    public class DBContextMySql : BaseDbContext
    {
        public DBContextMySql(DbContextOptions options, IConfiguration config) : base(options, config)
        {
            _connectionString = config.MyGetConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 28));

            options.UseMySql(_connectionString, serverVersion);

            base.OnConfiguring(options);
        }
    }

}
