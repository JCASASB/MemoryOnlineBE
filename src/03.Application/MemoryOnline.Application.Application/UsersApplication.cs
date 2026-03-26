using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
/*
namespace MemoryOnline.Application.Application
{
    public class UsersApplication
    {

        private readonly IGenericRepository<Usuario> _userRepository;

        public UsersApplication(IGenericRepository<Usuario> userRepository)
        {
            _userRepository = userRepository;
        }

        public void CreateUser(string nombre, string password)
        {
            var user = new Usuario { Name = nombre, Password = password };
            _userRepository.Add(user);
        }

        public Usuario GetUserByName(string name)
        {
            return _userRepository.Find(u => u.Name == name).First();
        }


        public Usuario GetUserById(Guid id)
        {
            return _userRepository.Find(u => u.Id == id).First();
        }


        public List<Usuario> GetAllUsers()
        {
            return _userRepository.GetAll().ToList();
        }

        public bool UpdateUser(Usuario user)
        {
            _userRepository.Update(user);
            return true;
        }

        public bool DeleteUser(string name)
        {
            var user = _userRepository.Find(u => u.Name == name).First();
            if (user == null) return false;
            _userRepository.Delete(user);
            return true;
        }
    }
}
*/