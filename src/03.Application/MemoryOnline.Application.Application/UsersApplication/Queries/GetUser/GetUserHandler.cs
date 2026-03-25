
using MediatR;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.UsersApplication.Queries.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, Usuario>
    {
        private readonly IUsersRepository _userRepository;

        public GetUserHandler(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Usuario> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetUserByName(request.name);
            return Task.FromResult(user);
        }
    }
}
