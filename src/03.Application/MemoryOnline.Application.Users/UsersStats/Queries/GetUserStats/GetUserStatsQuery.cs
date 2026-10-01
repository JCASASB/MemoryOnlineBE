using MediatR;
using MemoryOnline.Domain.Entities.Stats;

namespace MemoryOnline.Application.Users.UsersApplication.Queries.GetUser
{
    public record GetUserStatsQuery(Guid id) : IRequest<UserStats>;

}
