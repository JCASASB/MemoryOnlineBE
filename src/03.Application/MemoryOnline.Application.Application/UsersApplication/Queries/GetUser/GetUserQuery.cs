using MediatR;
using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Application.Application.UsersApplication.Queries.GetUser
{
    public record GetUserQuery(string name) : IRequest<Usuario>;

}
