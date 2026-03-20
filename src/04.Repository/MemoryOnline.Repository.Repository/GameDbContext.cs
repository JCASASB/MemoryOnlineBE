using MemoryOnline.Domain.Entities.Game;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Repository.Repository
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

        public DbSet<GameState> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameState>().HasKey(g => g.Id);
            modelBuilder.Entity<Player>().HasKey(p => p.Id);
            modelBuilder.Entity<Card>().HasKey(c => c.Id);
            modelBuilder.Entity<GameState>()
                .HasMany(g => g.Players)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<GameState>()
                .HasMany(g => g.Cards)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
