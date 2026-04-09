using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Configuration;

namespace MemoryOnline.Infraestructure.EF.Context
{
    public class ApplicationDbContextSqlServer : DbContext, IApplicationDbContext
    {
        public ApplicationDbContextSqlServer()
        {
        }

        public ApplicationDbContextSqlServer(DbContextOptions<ApplicationDbContextSqlServer> options) : base(options)
        {
        }

        public DbSet<GameState> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)

                // 2. Muestra los valores de los parámetros en las consultas SQL (crucial para debug)
                .EnableSensitiveDataLogging()

                // 3. Proporciona excepciones mucho más detalladas si falla la lectura de datos
                .EnableDetailedErrors();

                optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=GameDb;User Id=sa;Password=TuPasswordFuerte123!;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GameState>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.OwnsMany(g => g.Players, p => { p.ToJson(); });
                entity.OwnsMany(g => g.Cards, c => { c.ToJson(); });
            });
        }

        async Task<int> IApplicationDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
