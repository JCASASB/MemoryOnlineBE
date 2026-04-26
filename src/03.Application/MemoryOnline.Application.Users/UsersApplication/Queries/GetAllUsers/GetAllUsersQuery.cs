using MediatR;
using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Application.Users.UsersApplication.Queries.GetAllUsers
{
    public record GetAllUsersQuery() : IRequest<List<Usuario>>;
}
