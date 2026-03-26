using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Infraestructure.EF
{
    public interface IRepositoryUser
    {
        void AddUser(Usuario user);
        Usuario? GetUser(string name);
    }
}
