
using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Repository.IRepository
{
    public interface IRepositoryGame
    {
        GameState AddGame(GameState game);
        GameState? GetGame(Guid id);
        GameState? GetGameByName(string gameName);
        List<GameState> GetAllGames();
        void UpdateGame(GameState game);
        void DeleteGame(Guid id);
    }
}