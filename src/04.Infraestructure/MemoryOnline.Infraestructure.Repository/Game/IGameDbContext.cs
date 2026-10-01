using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;

namespace MemoryOnline.Infraestructure.IRepository.Game
{
    public interface IGameDbContext
    {
        IQueryable<Match> Matches { get; }
        IQueryable<Challenge> Challenges { get; }

        void AddMatch(Match match);
        void AddChallenge(Challenge challenge);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
