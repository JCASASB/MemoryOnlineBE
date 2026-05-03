using MemoryOnline.Infraestructure.IRepository.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MemoryOnline.Infraestructure.EF.Game.Context.ContextBases
{
    public class GameDbContextInMemory : GameDbContextBase
    {
        public GameDbContextInMemory(DbContextOptions<GameDbContextInMemory> options, IConfiguration config) : base(options, config)
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

                optionsBuilder.UseInMemoryDatabase(_connectionString);

                base.OnConfiguring(optionsBuilder);
            }
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
