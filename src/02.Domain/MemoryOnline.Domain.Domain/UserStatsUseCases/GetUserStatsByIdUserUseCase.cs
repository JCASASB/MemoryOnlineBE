using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOnline.Domain.Domain.UserStatsUseCases
{
    public class GetUserStatsByIdUserUseCase : IGetUserStatsByIdUserUseCase
    {
        public UserStats Execute(Guid userId, IEnumerable<UserMatchResult> results)
        {
            UserStats stats = new UserStats.Builder().WithId(userId).Build();

            foreach (var result in results)
            {
                stats.AddMatchs(result.Matchs);
                stats.AddMoves(result.Moves);
                stats.AddFails(result.Fails);
            }
            return stats;
        }
    }
}
