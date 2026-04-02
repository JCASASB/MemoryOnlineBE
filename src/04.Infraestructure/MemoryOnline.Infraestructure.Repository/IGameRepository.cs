using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Infraestructure.IRepository
{
    public interface IGameRepository
    {
        IEnumerable<GameState> GetAsync(Guid id);
        IEnumerable<GameState> GetGameByNameAsync(string gameName);
        IEnumerable<GameState> GetAllAsync();
        void AddAsync(GameState game);
        void UpdateAsync(GameState game);
        void DeleteAsync(Guid id);
    }
}
