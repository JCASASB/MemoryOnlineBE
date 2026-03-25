using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Infraestructure.IRepository
{
    public interface IGameRepository
    {
        void AddGame(GameState game);
        GameState? GetGame(System.Guid id);
        GameState? GetGameByName(string gameName);
        List<GameState> GetAllGames();
        void UpdateGame(GameState game);
        void DeleteGame(Guid id);
    }
}
