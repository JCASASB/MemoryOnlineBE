using MediatR;
using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Application.Application.UsersApplication.Queries.GetAllUsers
{
    public record GetAllUsersQuery() : IRequest<List<Usuario>>;
}
