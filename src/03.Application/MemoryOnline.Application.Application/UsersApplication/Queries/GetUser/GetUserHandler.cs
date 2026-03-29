
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

        public Task<Usuario> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetAll().Where(u => u.Name == request.name).ToList().First();
            return Task.FromResult(user);
        }
    }
}
