using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.IRepository;
using System.Linq.Expressions;

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

        public IEnumerable<Usuario> Get(Expression<Func<Usuario, bool>> filter = null, Func<IQueryable<Usuario>, IOrderedQueryable<Usuario>> orderBy = null, string includeProperties = "")
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Usuario> GetAll(Func<IQueryable<Usuario>, IOrderedQueryable<Usuario>> orderBy = null, string includeProperties = "")
        {
            throw new NotImplementedException();
        }

        public Usuario GetById(object id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Usuario> GetPagedElements<TKey>(int pageIndex, int pageCount, Expression<Func<Usuario, TKey>> orderByExpression, bool ascending = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Usuario> Find(Expression<Func<Usuario, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Usuario entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Attach(Usuario entityToAttach)
        {
            throw new NotImplementedException();
        }

        public void Update(Usuario entityToUpdate)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        IEnumerable<Usuario> IUsersRepository.GetAll()
        {
            return GetAll();
        }

        IEnumerable<Usuario> IUsersRepository.GetUserByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
