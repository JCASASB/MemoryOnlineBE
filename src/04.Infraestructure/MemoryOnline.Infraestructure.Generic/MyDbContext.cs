using MemoryOnline.Domain.Entities;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.Generic;
using MemoryOnline.Infraestructure.Generic.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.Repository
{
    public class MyDbContext(DbContextOptions options, IConfiguration config) : DBContextInMemory(options, config) , IMyDbContext
    {
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<BoardState> Games { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Card> Card { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
 /*
            // Configurar relaciones GameState -> Players
            modelBuilder.Entity<GameState>()
                .HasMany(g => g.Players)
                .WithOne(p => p.GameState)
                .HasForeignKey(p => p.GameStateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurar relaciones GameState -> Cards
            modelBuilder.Entity<GameState>()
                .HasMany(g => g.Cards)
                .WithOne(c => c.GameState)
                .HasForeignKey(c => c.GameStateId)
                .OnDelete(DeleteBehavior.Cascade);*/
        }
    }
}
