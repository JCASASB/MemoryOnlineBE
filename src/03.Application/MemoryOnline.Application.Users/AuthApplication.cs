using MemoryOnline.Domain.Domain.Specifications.Implementations;
using MemoryOnline.Domain.Entities.Users;
using MemoryOnline.Infraestructure.IRepository.Application;

namespace MemoryOnline.Application.Application
{
    public class AuthApplication
    {
        private readonly IApplicationUOW _userRepository;

        public AuthApplication(IApplicationUOW userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ValidateUser(string name, string password)
        {
            var filterSpec = new UserFilterByNameSpec(name);

            var users = await _userRepository.GetUserWithFilter(filterSpec);

            var user = users.FirstOrDefault<Usuario>();

            return user != null && user.Password == password;
        }
    }
}
