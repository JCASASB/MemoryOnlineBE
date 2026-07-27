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
                boardState.OwnsMany(bs => bs.Players);
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
