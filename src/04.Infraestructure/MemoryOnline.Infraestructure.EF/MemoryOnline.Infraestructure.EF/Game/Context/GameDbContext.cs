using Hispalance.Infraestructure.DB.DBContext;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Domain.Entities.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.EntityFrameworkCore.Extensions;

namespace MemoryOnline.Infraestructure.EF.Game.Context
{
    public class GameDbContext : DBContextMongoDB
    {
        public DbSet<Match> Matches { get; set; }


        public GameDbContext(IConfiguration config) : base(config)
        {
            Database.EnsureCreated();
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
        }
    }
}
