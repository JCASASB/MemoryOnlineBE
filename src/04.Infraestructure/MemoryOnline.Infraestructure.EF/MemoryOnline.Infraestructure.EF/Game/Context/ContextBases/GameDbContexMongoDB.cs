using MemoryOnline.Infraestructure.IRepository.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MemoryOnline.Infraestructure.EF.Game.Context.ContextBases
{
    public class GameDbContexMongoDB : GameDbContextBase
    {
        private string _database;

        public GameDbContexMongoDB(DbContextOptions<GameDbContexMongoDB> options, IConfiguration config) : base(options, config)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMongoDB(_connectionString, _database);

                base.OnConfiguring(optionsBuilder);
            }
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
