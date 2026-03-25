using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.Generic.DBContext
{
    public class DBContextInMemory : DBContextMyBase, IMyDbContext
    {

        public DBContextInMemory(IConfiguration config) : base(config)
        {
            _connectionString = GetConnectionString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("loquesea");

            base.OnConfiguring(options);
        }
    }

}
