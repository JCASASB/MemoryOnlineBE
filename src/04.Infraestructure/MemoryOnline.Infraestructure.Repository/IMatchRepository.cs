using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Infraestructure.IRepository
{
    public interface IMatchRepository
    {
        Task<IEnumerable<Match>> GetAllAsync();
        Task<Match> GetMatchByNameAsync(string name);
        Task<IEnumerable<BoardState>> GetAllBoardStatesAsync(Guid matchId);
        Task AddAsync(Match match);
        Task UpdateNewStateAsync(Guid matchId, BoardState game);
        Task UpdateAsync(Match match);
    }
}
