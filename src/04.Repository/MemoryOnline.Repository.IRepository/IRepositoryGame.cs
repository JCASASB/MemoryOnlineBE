

using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Repository.IRepository
{
    public interface IRepositoryGame
    {
        void AddGame(GameState game);
        GameState? GetGame(Guid id);
        GameState? GetGameByName(string gameName);
        List<GameState> GetAllGames();
        void UpdateGame(GameState game);
        void DeleteGame(Guid id);
    }
}