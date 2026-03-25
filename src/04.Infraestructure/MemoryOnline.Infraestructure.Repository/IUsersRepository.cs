using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Infraestructure.IRepository
{
    public interface IUsersRepository 
    {
        public void Add(Usuario entityToAdd);

        public List<Usuario> GetAll();

        public Usuario GetUserByName(string name);


    }
}
