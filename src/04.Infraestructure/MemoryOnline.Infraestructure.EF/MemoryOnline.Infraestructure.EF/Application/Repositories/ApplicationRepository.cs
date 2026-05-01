using Hispalance.Infraestructure.DB.IRepositories.Generic;
using MemoryOnline.Domain.Domain.Specifications.Interfaces;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;
using MemoryOnline.Infraestructure.IRepository.Application;

namespace MemoryOnline.Infraestructure.EF.Application.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {

        private readonly IGenericRepository<Usuario> _repository;

        private readonly IGenericRepository<UserResults> _repositoryResults;

        public ApplicationRepository(
            IGenericRepository<Usuario> repository
            , IGenericRepository<UserResults> repositoryResults)
        {
            _repository = repository;

            _repositoryResults = repositoryResults;
        }

        public async Task AddUserAsync(Usuario entityToAdd)
        {
            _repository.Add(entityToAdd);

            await _repository.SaveChangesAsync();
        }

        public async Task AddUserResultsAsync(UserResults results)
        {
            try
            {
                _repositoryResults.Add(results);

                await _repositoryResults.SaveChangesAsync();

            }
            catch (Exception ex) {
                throw new Exception($"Error al agregar los resultados del usuario: {ex.Message}", ex);
            }
        }

        public Task<IEnumerable<UserResults>> GetUserResultsWithFilterAsync(ISpecification<Guid> spec)
        {
            return _repositoryResults.GetAllAsync();
        }

        public async Task<IEnumerable<Usuario>> GetUserWithFilter(ISpecification<Usuario> spec)
        {
            Func<IQueryable<Usuario>, IOrderedQueryable<Usuario>> orderByFunc = null;

            if (spec.OrderBy != null)
            {
                orderByFunc = q => q.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                orderByFunc = q => q.OrderByDescending(spec.OrderByDescending);
            }

            // Llamamos a la sobrecarga del GetAll que definimos antes
            return await _repository.GetAllAsync(
                filter: spec.Criteria,               // El filtro (p => p.CategoryId...)
                orderBy: orderByFunc,               // El ordenamiento (si existe)
                includeProperties: spec.Includes.ToArray() // Convertimos la lista a Array para el 'params'
            );
        }



    }
}
