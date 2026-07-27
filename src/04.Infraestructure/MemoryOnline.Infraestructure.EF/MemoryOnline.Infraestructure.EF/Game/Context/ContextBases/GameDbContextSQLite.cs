using Hispalance.Infraestructure.DB.Implementations.DBContext;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.EF.Game.Context.ContextBases
{
    public class GameDbContextSQLite : DBContextSQLite
    {
        public GameDbContextSQLite(DbContextOptions options, IConfiguration config) : base(options, config)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Match>(entity =>
            {
                entity.ToTable("Matches");
                entity.HasKey(m => m.Id);

                // CONFIGURACIÓN JSON (Para simular comportamiento NoSQL en SQL Server)
                entity.OwnsMany(m => m.States, state =>
                {
                    state.ToJson(); // Convierte toda la colección States en una columna JSON

                    state.Property(s => s.Id).ValueGeneratedNever();

                    state.OwnsMany(s => s.Cards);
                    state.OwnsMany(s => s.Players);
                });
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(u => u.Id);

                // Definir la relación 1 a muchos
                entity.HasMany(u => u.Results)         // Un Usuario tiene muchos Results
                      .WithOne()                       // Cada Result pertenece a un Usuario
                      .HasForeignKey(r => r.UsuarioId) // FK usando la propiedad real de la entidad
                      .OnDelete(DeleteBehavior.Cascade); // Si borras al usuario, se borran sus resultados
            });

modelBuilder.Entity<UserMatchResult>(entity =>
{
    entity.ToTable("UsuarioResults");
    entity.HasKey(r => r.Id);

    entity.Property(r => r.Id).ValueGeneratedNever();
    entity.Property(r => r.MatchId).IsRequired();
    entity.Property(r => r.UsuarioId).IsRequired();
    entity.Property(r => r.Moves).IsRequired();
    entity.Property(r => r.Fails).IsRequired();
    entity.Property(r => r.Matchs).IsRequired();
    entity.Property(r => r.Winner).IsRequired();

    entity.HasOne<Match>()
          .WithMany()
          .HasForeignKey(r => r.MatchId)
          .OnDelete(DeleteBehavior.Cascade);
});

modelBuilder.Entity<Challenge>(entity =>
{
    entity.ToTable("Challenges");
    entity.HasKey(c => c.Id);

    entity.Property(c => c.Id).ValueGeneratedNever();

    entity.OwnsMany(c => c.Players, player =>
    {
        player.ToJson();
    });

    entity.HasOne(c => c.Match)
          .WithMany()
          .HasForeignKey("MatchId")
          .OnDelete(DeleteBehavior.Cascade);
});


        }

    }
}
