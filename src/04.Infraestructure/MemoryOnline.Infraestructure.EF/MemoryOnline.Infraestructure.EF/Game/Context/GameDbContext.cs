using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;
using MemoryOnline.Infraestructure.EF.Game.Context.ContextBases;
using MemoryOnline.Infraestructure.IRepository.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.EF.Game.Context
{
    public class GameDbContext : GameDbContextSQLite, IGameDbContext
    {
        public DbSet<Match> Matches { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<UserMatchResult> UsuarioResults { get; set; }

        public GameDbContext(DbContextOptions options, IConfiguration config) : base(options, config)
        {
           // Database.EnsureDeleted(); // Agregado para forzar la eliminación de la BD obsoleta
            Database.EnsureCreated();
        }

        
    }
}
