using MemoryOnline.Domain.Domain.Specifications.Interfaces;
using MemoryOnline.Domain.Entities;
using System.Threading.Tasks;

namespace MemoryOnline.Infraestructure.IRepository 
{
    public interface IUsersRepository 
    {
        Task<IEnumerable<Usuario>> GetWithFilter(ISpecification<Usuario> spec);
        Task AddAsync(Usuario entityToAdd);
    }
}
