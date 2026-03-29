
using MediatR;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.UsersApplication.Queries.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, Usuario>
    {
        private readonly IGenericRepository<Usuario> _userRepository;

        public GetUserHandler(IGenericRepository<Usuario> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Usuario> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.Where(u => u.Name == request.name).First();
            return user;
        }
    }
}
