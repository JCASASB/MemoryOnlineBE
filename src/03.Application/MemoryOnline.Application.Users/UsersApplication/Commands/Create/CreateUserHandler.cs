using MediatR;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Users.UsersApplication.Commands.Create
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Usuario>
    {
        private readonly IUsersRepository _userRepository;

        public CreateUserHandler(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Usuario> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Usuario.Builder()
                                        .WithName(request.userName)
                                        .WithPassword(request.password)
                                        .Build();

            await _userRepository.AddAsync(user);

            return user;
        }
    }
}
