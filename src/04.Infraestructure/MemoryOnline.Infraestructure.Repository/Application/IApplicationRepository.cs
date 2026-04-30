using MemoryOnline.Domain.Domain.Specifications.Interfaces;
using MemoryOnline.Domain.Entities.Users;

namespace MemoryOnline.Infraestructure.IRepository.Application 
{
    public interface IApplicationRepository 
    {
        Task<IEnumerable<Usuario>> GetWithFilter(ISpecification<Usuario> spec);
        Task AddAsync(Usuario entityToAdd);
    }
}
