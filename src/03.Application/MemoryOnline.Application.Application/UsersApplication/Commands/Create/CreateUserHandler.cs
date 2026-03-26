using MediatR;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.UsersApplication.Commands.Create
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IUsersRepository _userRepository;

        public CreateUserHandler(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var name = request.userDto.GetProperty("username").GetString();
            var password = request.userDto.GetProperty("password").GetString();

            var user = new Usuario { Name = name, Password = password };
            _userRepository.Add(user);

            await Task.CompletedTask;
        }
    }
}
