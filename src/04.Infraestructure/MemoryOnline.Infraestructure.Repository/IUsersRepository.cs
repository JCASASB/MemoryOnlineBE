using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;

namespace MemoryOnline.Infraestructure.IRepository
{
    public interface IUsersRepository 
    {
        IEnumerable<Usuario> GetAll();
        IEnumerable<Usuario> GetUserByName(string name);
        void Add(Usuario entityToAdd);

    }
}
