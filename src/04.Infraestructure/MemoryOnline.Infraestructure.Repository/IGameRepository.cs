using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Infraestructure.IRepository
{
    public interface IGameRepository
    {
        IEnumerable<GameState> Get(Guid id);
        IEnumerable<GameState> GetGameByName(string gameName);
        IEnumerable<GameState> GetAll();
        void Add(GameState game);
        void Update(GameState game);
        void Delete(Guid id);
    }
}
