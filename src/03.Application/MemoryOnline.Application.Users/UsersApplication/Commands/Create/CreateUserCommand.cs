using MediatR;
using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Application.Users.UsersApplication.Commands.Create
{
    public record CreateUserCommand(string userName, string password) : IRequest<Usuario>;

}
