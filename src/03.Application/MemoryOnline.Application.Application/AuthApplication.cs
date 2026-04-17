using MemoryOnline.Domain.Domain.Specifications.Implementations;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application
{
    public class AuthApplication
    {
        private readonly IUsersRepository _userRepository;

        public AuthApplication(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ValidateUser(string name, string password)
        {
            var filterSpec = new UserFilterByNameSpec(name);

            var users = await _userRepository.GetWithFilter(filterSpec);

            var user = users.FirstOrDefault<Usuario>();

            return user != null && user.Password == password;
        }
    }
}
