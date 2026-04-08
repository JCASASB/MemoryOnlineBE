using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Infraestructure.IRepository
{
    public interface IGameRepository
    {
        Task<IEnumerable<GameState>> GetAsync(Guid id);
        Task<IEnumerable<GameState>> GetGameByNameAsync(string gameName);
        Task<IEnumerable<GameState>> GetAllAsync();
        Task AddAsync(GameState game);
        Task UpdateAsync(GameState game);
        Task DeleteAsync(Guid id);
    }
}
