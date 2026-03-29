using MediatR;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;

namespace MemoryOnline.Application.Application.UsersApplication.Commands.Create
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IGenericRepository<Usuario> _userRepository;

        public CreateUserHandler(IGenericRepository<Usuario> userRepository)
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
