using Hispalance.Infraestructure.DB.DBContext;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;
using MemoryOnline.Infraestructure.IRepository.Game;
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
                    state.OwnsMany(s => s.Players);
                });
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);

                // Definir la relación 1 a muchos
                entity.HasMany(u => u.Results)         // Un Usuario tiene muchos Results
                      .WithOne()                       // Cada Result pertenece a un Usuario
                      .HasForeignKey("UsuarioId")      // EF creará esta Shadow Property en la DB
                      .OnDelete(DeleteBehavior.Cascade); // Si borras al usuario, se borran sus resultados
            });

            modelBuilder.Entity<UserMatchResult>(entity =>
            {
                entity.HasKey(r => r.Id); // Asumiendo que UserMatchResult tiene un Id
                entity.Property(r => r.UsuarioId).IsRequired();
            });
        }

    }
}
