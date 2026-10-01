using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.EF.Game.Context;
using MemoryOnline.Infraestructure.EF.Game.Repositories;
using MemoryOnline.Infraestructure.IRepository.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MemoryOnline.Infraestructure.EFTests;

public class GameRepositoryInMemoryTests : IDisposable
{
    private readonly GameDbContext _context;
    private readonly IGameRepository _repository;

    public GameRepositoryInMemoryTests()
    {
        var databaseName = $"MemoryOnlineTests-{Guid.NewGuid()}";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DBSection:InMemoryConnectionString"] = databaseName
            })
            .Build();

        _context = new GameDbContext(
            new DbContextOptionsBuilder<GameDbContext>().Options,
            configuration);

        _repository = new GameRepositoryEF(_context);
    }

    [Fact]
    public async Task AddMatchAsync_PersistsMatch()
    {
        var match = CreateMatch();

        await _repository.AddMatchAsync(match);

        var stored = await _repository.GetMatchByIdAsync(match.Id);
        Assert.NotNull(stored);
        Assert.Equal(match.Name, stored.Name);
    }

    [Fact]
    public async Task AddChallengeAsync_PersistsPlayersAndMatch()
    {
        var match = CreateMatch();
        await _repository.AddMatchAsync(match);

        var challenge = new Challenge
        {
            Id = Guid.NewGuid(),
            Match = match,
            Players =
            [
                new Player { Id = Guid.NewGuid(), Name = "Player 1" },
                new Player { Id = Guid.NewGuid(), Name = "Player 2" }
            ],
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddChallengeAsync(challenge);
        _context.ChangeTracker.Clear();

        var stored = await _repository.GetChallengeAsync(challenge.Id);

        Assert.NotNull(stored);
        Assert.Equal(2, stored.Players.Count);
        Assert.Equal(match.Id, stored.Match.Id);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    private static Match CreateMatch() => new()
    {
        Id = Guid.NewGuid(),
        Name = "Test match",
        Level = 1,
        States = []
    };
}
