using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Infraestructure.IRepository.Game
{
    public interface IGameRepository
    {
        Task<IEnumerable<Match>> GetAllMatchAsync();
        Task<Match> GetMatchByNameAsync(string name);
        Task<IEnumerable<BoardState>> GetAllBoardStatesAsync(Guid matchId);
        Task AddMatchAsync(Match match);
        Task UpdateNewStateAsync(Guid matchId, BoardState game);
        Task UpdateMatchAsync(Match match);
        Task AddChallengeAsync(Challenge challenge);
        Task<Challenge> GetChallengeAsync(Guid challengeId);
        Task<Match> GetMatchByIdAsync(Guid matchId);

    }
}
