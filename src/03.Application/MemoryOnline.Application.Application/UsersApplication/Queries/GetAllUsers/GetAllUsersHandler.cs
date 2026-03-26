using MediatR;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.UsersApplication.Queries.GetAllUsers
{
  public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<Usuario>>
    {
        private readonly IUsersRepository _userRepository;

        public GetAllUsersHandler(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<List<Usuario>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            var users = _userRepository.GetAll().ToList();
            return Task.FromResult(users);
        }
    }
}
