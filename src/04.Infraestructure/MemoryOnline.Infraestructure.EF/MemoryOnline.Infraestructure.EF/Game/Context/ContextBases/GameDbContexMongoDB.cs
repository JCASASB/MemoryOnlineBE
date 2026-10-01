using Hispalance.Infraestructure.DB.DBContext;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.EntityFrameworkCore.Extensions;

namespace MemoryOnline.Infraestructure.EF.Game.Context.ContextBases
{
    public class GameDbContexMongoDB : DBContextMongoDB
    {
        public GameDbContexMongoDB(DbContextOptions options, IConfiguration config) : base(options, config)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Match>(entity =>
            {
                entity.ToCollection("Matches");
                entity.HasKey(m => m.Id);

                // CONFIGURACIÓ PER A MONGODB
                entity.OwnsMany(m => m.States, state =>
                {
                    // 1. ELIMINA EL .HasKey(). En Mongo, els elements de la llista 
                    // es tracten com a objectes sense identitat pròpia per a EF Core.

                    // 2. Que l'ID es guardi com a string o Guid normal:
                    state.Property(s => s.Id).ValueGeneratedNever();

                    // 3. Configurem els sub-nivells (també sense HasKey)
                    state.OwnsMany(s => s.Cards);
                    state.OwnsMany(s => s.Players, player =>
                    {
                        player.OwnsMany(p => p.Jokers);
                    });
                });
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToCollection("Usuarios");
                entity.HasKey(u => u.Id);
                entity.Ignore(u => u.Results);
            });

            modelBuilder.Entity<UserMatchResult>(entity =>
            {
                entity.ToCollection("UserMatchResults");
                entity.HasKey(r => r.Id); // Asumiendo que UserMatchResult tiene un Id
                entity.Property(r => r.UsuarioId).IsRequired();
            });

            modelBuilder.Entity<Challenge>(entity =>
            {
                entity.ToCollection("Challenges");
                entity.HasKey(c => c.Id);
                entity.OwnsMany(c => c.Players, player =>
                {
                    player.OwnsMany(p => p.Jokers);
                });
                entity.Property(c => c.MatchId).IsRequired();
                entity.Ignore(c => c.Match);
            });
        }

    }
}
