using MemoryOnline.Domain.Entities;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MemoryOnline.Repository.Repository
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) {

            //TODO quitar esto , se ejecuta cada vez que se inicia la aplicación, solo para desarrollo
            Database.EnsureCreated();
        
        }

        public DbSet<GameState> Games => Set<GameState>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Card> Cards => Set<Card>();
        public DbSet<Usuario> Users => Set<Usuario>();

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

            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);

        }
    }
}
