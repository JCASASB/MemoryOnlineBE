using MediatR;
using MemoryOnline.Domain.Entities.Users;
using MemoryOnline.Infraestructure.IRepository.Application;

namespace MemoryOnline.Application.Users.UsersApplication.Queries.GetAllUsers
{
  public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<Usuario>>
    {
        private readonly IApplicationUOW _userRepository;

        public GetAllUsersHandler(IApplicationUOW userRepository)
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
