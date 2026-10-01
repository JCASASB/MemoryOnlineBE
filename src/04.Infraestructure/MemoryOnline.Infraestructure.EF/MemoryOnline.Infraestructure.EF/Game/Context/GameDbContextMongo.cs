using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;
using MemoryOnline.Infraestructure.EF.Game.Context.ContextBases;
using MemoryOnline.Infraestructure.IRepository.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.EF.Game.Context;

public class GameDbContextMongo : GameDbContexMongoDB, IGameDbContext
{
    public DbSet<Match> Matches { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Challenge> Challenges { get; set; }
    public DbSet<UserMatchResult> UsuarioResults { get; set; }

    public GameDbContextMongo(DbContextOptions options, IConfiguration config)
        : base(options, config)
    {
    }

    IQueryable<Match> IGameDbContext.Matches => Matches;
    IQueryable<Challenge> IGameDbContext.Challenges => Challenges;

    public void AddMatch(Match match) => Matches.Add(match);
    public void AddChallenge(Challenge challenge) => Challenges.Add(challenge);
}
