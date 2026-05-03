using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.EF.Game.Context.ContextBases
{
    public abstract class GameDbContextBase : DbContext
    {
        protected IConfiguration _config;
        protected string _connectionString;

        protected GameDbContextBase(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
            _connectionString = GetConnectionString();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected virtual string GetConnectionString()
        {
            try
            {
                var server = _config.GetSection("DBSection:Server").Value;
                var port = _config.GetSection("DBSection:Port").Value;
                var database = _config.GetSection("DBSection:Database").Value;
                var user = _config.GetSection("DBSection:User").Value;
                var pass = _config.GetSection("DBSection:Password").Value;
                var connectionString = String.Format("server={0};Port={1};database={2};uid={3};pwd={4}",
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
