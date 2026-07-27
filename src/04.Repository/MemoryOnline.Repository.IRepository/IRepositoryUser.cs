using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Repository.IRepository
{
    public interface IRepositoryUser
    {
        void AddUser(Usuario user);
        Usuario? GetUser(string name);
    }
}
