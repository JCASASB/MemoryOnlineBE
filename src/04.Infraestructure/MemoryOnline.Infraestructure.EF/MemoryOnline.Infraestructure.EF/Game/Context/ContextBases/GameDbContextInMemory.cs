using Hispalance.Infraestructure.DB.DBContext;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.EF.Game.Context.ContextBases
{
    public class GameDbContextInMemory : DBContextInMemory
    {
        public GameDbContextInMemory(DbContextOptions options, IConfiguration config) : base(options, config)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.OwnsMany(m => m.States, state =>
                {
                    state.Property(s => s.Id).ValueGeneratedNever();
                    state.HasKey(s => s.Id);

                    state.OwnsMany(s => s.Cards, card =>
                    {
                        card.HasKey("Id");
                    });

                    state.OwnsMany(s => s.Players, player =>
                    {
                        player.HasKey("Id");
                    });
                });
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.HasMany(u => u.Results)
                      .WithOne()
                      .HasForeignKey(r => r.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserMatchResult>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.UsuarioId).IsRequired();
            });
        }

    }
}
