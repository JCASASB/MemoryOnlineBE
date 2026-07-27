using MemoryOnline.Domain.Entities.Stats;

namespace MemoryOnline.Domain.Domain.UserStatsUseCases
{
    public interface IGetUserStatsByIdUserUseCase
    {
        UserStats Execute(Guid userId, IEnumerable<UserMatchResult> results);

    }
}
