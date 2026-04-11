using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.EF.Context;
using MemoryOnline.Infraestructure.EF.Repositories;
using MemoryOnline.Infraestructure.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using MemoryOnline.Common.IOC;

namespace MemoryOnline.Infraestructure.EFTests;

public class GameRepositoryInMemoryTests : IDisposable
{
    private readonly ApplicationDbContextInMemory _context;
    private readonly IMatchRepository _repository;
    private readonly ServiceProvider _serviceProvider;

    public GameRepositoryInMemoryTests()
    {
        var services = new ServiceCollection();

        services.AddEFInMemory();

        _serviceProvider = services.BuildServiceProvider();

        _context = _serviceProvider.GetRequiredService<ApplicationDbContextInMemory>();
        _repository = _serviceProvider.GetRequiredService<IMatchRepository>();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _serviceProvider.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task AddAsync_ShouldAddGame_WhenValidGameProvided()
    {
        // Arrange
        var game = CreateTestGame();

        // Act
        await _repository.AddAsync(game);

        // Assert
        var result = _context.Games.FirstOrDefault(g => g.Id == game.Id);
        Assert.NotNull(result);
        Assert.Equal(game.Name, result.Name);
        Assert.Equal(game.Level, result.Level);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnGame_WhenGameExists()
    {
        // Arrange
        var game = CreateTestGame();
        _context.Games.Add(game);
        _context.SaveChanges();

        // Act
        var result = await _repository.GetAsync(game.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(game.Id, result.First().Id);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnEmpty_WhenGameNotExists()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetAsync(nonExistentId);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAsync_ShouldIncludePlayers_WhenGameHasPlayers()
    {
        // Arrange
        var game = CreateTestGame();
        game.Players.Add(new Player { Id = Guid.NewGuid(), Name = "Player1" });
        game.Players.Add(new Player { Id = Guid.NewGuid(), Name = "Player2" });
        _context.Games.Add(game);
        _context.SaveChanges();

        // Act
        var result = await _repository.GetAsync(game.Id);
        var gameResult = result.First();

        // Assert
        Assert.Equal(2, gameResult.Players.Count);
    }

    [Fact]
    public async Task GetAsync_ShouldIncludeCards_WhenGameHasCards()
    {
        // Arrange
        var game = CreateTestGame();
        game.Cards.Add(new Card { Id = Guid.NewGuid(), Value = 1 });
        game.Cards.Add(new Card { Id = Guid.NewGuid(), Value = 2 });
        _context.Games.Add(game);
        _context.SaveChanges();

        // Act
        var result = await _repository.GetAsync(game.Id);
        var gameResult = result.First();

        // Assert
        Assert.Equal(2, gameResult.Cards.Count);
    }

    [Fact]
    public async Task GetGameByNameAsync_ShouldReturnGames_WhenNameMatches()
    {
        // Arrange
        var gameName = "TestGame";
        var game1 = CreateTestGame(gameName);
        var game2 = CreateTestGame(gameName);
        var game3 = CreateTestGame("OtherGame");
        _context.Games.AddRange(game1, game2, game3);
        _context.SaveChanges();

        // Act
        var result = await _repository.GetGameByNameAsync(gameName);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, g => Assert.Equal(gameName, g.Name));
    }

    [Fact]
    public async Task GetGameByNameAsync_ShouldReturnEmpty_WhenNoMatchingName()
    {
        // Arrange
        var game = CreateTestGame("ExistingGame");
        _context.Games.Add(game);
        _context.SaveChanges();

        // Act
        var result = await _repository.GetGameByNameAsync("NonExistentGame");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllGames()
    {
        // Arrange
        var game1 = CreateTestGame("Game1");
        var game2 = CreateTestGame("Game2");
        var game3 = CreateTestGame("Game3");
        _context.Games.AddRange(game1, game2, game3);
        _context.SaveChanges();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmpty_WhenNoGamesExist()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateGame_WhenGameExists()
    {
        // Arrange
        var game = CreateTestGame();
        _context.Games.Add(game);
        _context.SaveChanges();
        _context.Entry(game).State = EntityState.Detached;

        var updatedGame = new BoardState
        {
            Id = game.Id,
            Name = "UpdatedName",
            Level = 5,
            Version = 2
        };

        // Act
        await _repository.UpdateAsync(updatedGame);

        // Assert
        var result = _context.Games.Find(game.Id);
        Assert.NotNull(result);
        Assert.Equal("UpdatedName", result.Name);
        Assert.Equal(5, result.Level);
        Assert.Equal(2, result.Version);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveGame_WhenGameExists()
    {
        // Arrange
        var game = CreateTestGame();
        _context.Games.Add(game);
        _context.SaveChanges();

        // Act
        await _repository.DeleteAsync(game.Id);

        // Assert
        var result = _context.Games.Find(game.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotThrow_WhenGameNotExists()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var exception = await Record.ExceptionAsync(async () => await _repository.DeleteAsync(nonExistentId));
        Assert.Null(exception);
    }

    private static BoardState CreateTestGame(string name = "TestGame")
    {
        return new BoardState
        {
            Id = Guid.NewGuid(),
            Name = name,
            Level = 1,
            Version = 1,
            Cards = [],
            Players = []
        };
    }
}
