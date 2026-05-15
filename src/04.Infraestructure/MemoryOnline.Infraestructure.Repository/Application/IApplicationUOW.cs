using MemoryOnline.Domain.Domain.Specifications.Interfaces;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;

namespace MemoryOnline.Infraestructure.IRepository.Application 
{
    public interface IApplicationUOW 
    {
        Task<IEnumerable<Usuario>> GetUserWithFilter(ISpecification<Usuario> spec);

        Task AddUserAsync(Usuario entityToAdd);

        Task<IEnumerable<UserMatchResult>> GetUserResultsWithFilterAsync(ISpecification<UserMatchResult> spec);

        Task AddUserResultsAsync(UserMatchResult results);


    }
}
