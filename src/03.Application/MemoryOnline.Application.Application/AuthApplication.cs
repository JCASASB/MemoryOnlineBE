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

        public Boolean ValidateUser(string name, string password)
        {
            var user = _userRepository.GetUserByName(name);

            return user != null && user.Password == password;
        }
    }
}
