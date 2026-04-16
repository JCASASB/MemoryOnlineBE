using Hispalance.Infraestructure.DB.IRepositories.Generic;
using MemoryOnline.Domain.Domain.Specifications.Interfaces;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Infraestructure.EF.Users
{
    public class UsersRepository : IUsersRepository
    {

        private readonly IGenericRepository<Usuario> _repository;

        public UsersRepository(IGenericRepository<Usuario> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Usuario entityToAdd)
        {
            _repository.Add(entityToAdd);

            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> GetWithFilter(ISpecification<Usuario> spec)
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
