using MemoryOnline.Domain.Domain.Specifications.Interfaces;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;

namespace MemoryOnline.Infraestructure.IRepository.Application 
{
    public interface IApplicationRepository 
    {
        Task<IEnumerable<Usuario>> GetUserWithFilter(ISpecification<Usuario> spec);

        Task AddUserAsync(Usuario entityToAdd);


        Task<IEnumerable<UserResults>> GetUserResultsWithFilterAsync(ISpecification<Guid> spec);

        Task AddUserResultsAsync(UserResults results);


    }
}
