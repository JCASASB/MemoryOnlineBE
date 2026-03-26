using MediatR;

namespace MemoryOnline.Application.Application.UsersApplication.Commands.Create
{
    public record CreateUserCommand(dynamic userDto) : IRequest;

}
