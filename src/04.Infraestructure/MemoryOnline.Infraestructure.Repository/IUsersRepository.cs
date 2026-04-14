using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Infraestructure.IRepository 
{
    public interface IUsersRepository 
    {
        IEnumerable<Usuario> GetAll();
        IEnumerable<Usuario> GetUserByName(string name);
        void Add(Usuario entityToAdd);

    }
}
