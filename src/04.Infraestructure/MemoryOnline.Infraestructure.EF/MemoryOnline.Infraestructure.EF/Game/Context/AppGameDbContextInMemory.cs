using MemoryOnline.Infraestructure.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MemoryOnline.Infraestructure.EF.Game.Context
{
    public class AppGameDbContextInMemory : AppGameDbContextBase, IApplicationDbContext
    {
        public AppGameDbContextInMemory(DbContextOptions<AppGameDbContextInMemory> options) : base(options)
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

                optionsBuilder.UseInMemoryDatabase("TuCadenaDeConexion");
            }
        }

        async Task<int> IApplicationDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
