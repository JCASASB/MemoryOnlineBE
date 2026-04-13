using MemoryOnline.Domain.Entities.Game;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace MemoryOnline.Infraestructure.EF.Context
{
    public abstract class ApplicationDbContextBase : DbContext
    {
        protected ApplicationDbContextBase(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Match> Matches { get; set; }

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
