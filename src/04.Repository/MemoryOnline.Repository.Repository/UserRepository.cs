using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Repository.Repository
{
    public class UserRepository : IUsersRepository
    {
        private readonly GameDbContext _context;

        public UserRepository(GameDbContext context)
        {
            _context = context;
        }

        public void Add(Usuario user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public Usuario? GetUser(string name)
        {
            return _context.Users.FirstOrDefault(u => u.Name == name);
        }

        public List<Usuario> GetAll()
        {
            return _context.Users.ToList();
        }

        public bool UpdateUser(Usuario user)
        {
            var existing = _context.Users.FirstOrDefault(u => u.Name == user.Name);
            if (existing == null) return false;
            existing.Age = user.Age;
            existing.Password = user.Password;
            _context.SaveChanges();
            return true;
        }

        public bool DeleteUser(string name)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == name);
            if (user == null) return false;
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public Usuario GetUserByName(string name)
        {
            return _context.Users.First(u => u.Name == name);
        }
    }
}
