using MediatR;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Users.UsersApplication.Queries.GetAllUsers
{
  public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<Usuario>>
    {
        private readonly IUsersRepository _userRepository;

        public GetAllUsersHandler(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<Usuario>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
          //  var users = await _userRepository.GetAllAsync();
           // return users.ToList();
        }
    }
}
