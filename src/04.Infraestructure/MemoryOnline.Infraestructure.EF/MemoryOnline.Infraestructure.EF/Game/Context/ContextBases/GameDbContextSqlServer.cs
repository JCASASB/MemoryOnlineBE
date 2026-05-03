using MemoryOnline.Infraestructure.IRepository.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MemoryOnline.Infraestructure.EF.Game.Context.ContextBases
{
    public class GameDbContextSqlServer : GameDbContextBase
    {
        public GameDbContextSqlServer(DbContextOptions<GameDbContextSqlServer> options, IConfiguration config) : base(options, config)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)

                // 2. Muestra los valores de los parámetros en las consultas SQL (crucial para debug)
                .EnableSensitiveDataLogging()

                // 3. Proporciona excepciones mucho más detalladas si falla la lectura de datos
                .EnableDetailedErrors();

                optionsBuilder.UseSqlServer(_connectionString);

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
