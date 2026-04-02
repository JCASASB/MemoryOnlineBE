using MediatR;
using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Application.Users.UsersApplication.Queries.GetUser
{
    public record GetUserQuery(string name) : IRequest<Usuario>;

}
