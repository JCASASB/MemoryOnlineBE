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
        public virtual DbSet<GameState> GameState { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Card> Card { get; set; }

       
    }
}
