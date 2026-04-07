using MemoryOnline.Domain.Entities.Game;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.EF.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<GameState> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TuCadenaDeConexion");
        }
    }
}
