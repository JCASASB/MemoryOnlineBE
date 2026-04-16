using MediatR;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Users.UsersApplication.Commands.Create
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

            var user = new Usuario.Builder()
                                        .WithName(name)
                                        .WithPassword(password)
                                        .Build();

            await _userRepository.AddAsync(user);
        }
    }
}
