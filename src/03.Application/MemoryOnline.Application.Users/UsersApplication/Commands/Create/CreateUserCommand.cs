using MediatR;

namespace MemoryOnline.Application.Users.UsersApplication.Commands.Create
{
    public record CreateUserCommand(dynamic userDto) : IRequest;

}
