
using MediatR;
using MemoryOnline.Domain.Domain.Specifications.Implementations;
using MemoryOnline.Domain.Entities.Users;
using MemoryOnline.Infraestructure.IRepository.Application;

namespace MemoryOnline.Application.Users.UsersApplication.Queries.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, Usuario>
    {
        private readonly IApplicationRepository _userRepository;

        public GetUserHandler(IApplicationRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Usuario> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterSpec = new UserFilterByNameSpec(request.name);

                var users = await _userRepository.GetUserWithFilter(filterSpec);

                Usuario? user = null;
                using (var e = users.GetEnumerator())
                {
                    if (e.MoveNext())
                        user = e.Current;
                }

                return user!;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
           
        }
    }
}
