using MediatR;
using MemoryOnline.Domain.Entities.Users;

namespace MemoryOnline.Application.Users.UsersApplication.Queries.GetAllUsers
{
    public record GetAllUsersQuery() : IRequest<List<Usuario>>;
}
