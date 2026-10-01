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

            modelBuilder.Entity<Match>()
            .OwnsMany(m => m.States, boardState =>
            {
                boardState.OwnsMany(bs => bs.Cards);
                boardState.OwnsMany(bs => bs.Players, player =>
                {
                    player.OwnsMany(p => p.Jokers);
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

            modelBuilder.Entity<Challenge>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.OwnsMany(c => c.Players, player =>
                {
                    player.OwnsMany(p => p.Jokers);
                });

                entity.HasOne(c => c.Match)
                      .WithMany()
                      .HasForeignKey(c => c.MatchId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
