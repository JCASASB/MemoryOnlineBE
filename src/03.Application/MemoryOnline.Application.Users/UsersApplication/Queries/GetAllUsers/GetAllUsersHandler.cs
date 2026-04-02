using MediatR;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;

namespace MemoryOnline.Application.Users.UsersApplication.Queries.GetAllUsers
{
  public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<Usuario>>
    {
        private readonly IGenericRepository<Usuario> _userRepository;

        public GetAllUsersHandler(IGenericRepository<Usuario> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<Usuario>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            return users.ToList();
        }
    }
}
