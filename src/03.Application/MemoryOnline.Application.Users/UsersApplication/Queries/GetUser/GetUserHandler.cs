
using MediatR;
using MemoryOnline.Domain.Domain.Specifications.Implementations;
using System.Linq;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Users.UsersApplication.Queries.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, Usuario>
    {
        private readonly IUsersRepository _userRepository;

        public GetUserHandler(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Usuario> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var us = new Usuario.Builder()
                .WithName(request.name)
                .WithAge(20)
                .WithPassword("temp")
                .Build();

          //  await _userRepository.AddAsync(us);


            var filterSpec = new UserFilterByNameSpec(request.name);

            var users = await _userRepository.GetWithFilter(filterSpec);

            Usuario? user = null;
            using (var e = users.GetEnumerator())
            {
                if (e.MoveNext())
                    user = e.Current;
            }

            return user!;
        }
    }
}
