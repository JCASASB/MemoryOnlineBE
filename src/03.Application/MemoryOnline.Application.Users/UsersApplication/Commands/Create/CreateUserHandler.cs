using MediatR;
using MemoryOnline.Domain.Entities.Users;
using MemoryOnline.Infraestructure.IRepository.Application;

namespace MemoryOnline.Application.Users.UsersApplication.Commands.Create
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Usuario>
    {
        private readonly IApplicationRepository _userRepository;

        public CreateUserHandler(IApplicationRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Usuario> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Usuario.Builder()
                                        .WithName(request.userName)
                                        .WithPassword(request.password)
                                        .Build();

            await _userRepository.AddUserAsync(user);

            return user;
        }
    }
}
