using MemoryOnline.Domain.Entities;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.Generic;
using Microsoft.EntityFrameworkCore;

namespace MemoryOnline.Infraestructure.Repository
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options) , IMyDbContext
    {
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<GameState> GameState { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Card> Card { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("loquesea");

            base.OnConfiguring(options);
        }
    }
}
