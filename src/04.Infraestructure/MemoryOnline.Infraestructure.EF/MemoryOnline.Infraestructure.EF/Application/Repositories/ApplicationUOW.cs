using Hispalance.Infraestructure.DB.IRepositories.Generic;
using MemoryOnline.Domain.Domain.Specifications.Interfaces;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;
using MemoryOnline.Infraestructure.IRepository.Application;

namespace MemoryOnline.Infraestructure.EF.Application.Repositories
{
    public class ApplicationUOW : IApplicationUOW
    {

        private readonly IGenericRepository<Usuario> _repository;

        private readonly IGenericRepository<UserMatchResult> _repositoryResults;

        public ApplicationUOW(
            IGenericRepository<Usuario> repository
            , IGenericRepository<UserMatchResult> repositoryResults)
        {
            _repository = repository;

            _repositoryResults = repositoryResults;
        }

        public async Task AddUserAsync(Usuario entityToAdd)
        {
            _repository.Add(entityToAdd);

            await _repository.SaveChangesAsync();
        }

        public async Task AddUserResultsAsync(UserMatchResult results)
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

        /*
         * En Ef, mongodb no permite hacer includes a no ser que esten configuradas como embebed las tablas
         * relacionadas.Aqui uso userresults por un lado, que es el "modulo" game quien guarda esos
         * datos con un evento, sin tocar la tabla usuario. Es decir de forma separada. 
         * 
         * Para mantenerlo así y seguir usando mongo, tengo que hacer este.
         */
        public async Task<IEnumerable<UserMatchResult>> GetUserResultsWithFilterAsync(ISpecification<UserMatchResult> spec)
        {
            Func<IQueryable<UserMatchResult>, IOrderedQueryable<UserMatchResult>> orderByFunc = null;

            if (spec.OrderBy != null)
            {
                orderByFunc = q => q.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                orderByFunc = q => q.OrderByDescending(spec.OrderByDescending);
            }

            // Llamamos a la sobrecarga del GetAll que definimos antes
            return await _repositoryResults.GetAllAsync(
                filter: spec.Criteria,               // El filtro (p => p.CategoryId...)
                orderBy: orderByFunc,               // El ordenamiento (si existe)
                includeProperties: spec.Includes.ToArray() // Convertimos la lista a Array para el 'params'
            );

        }



    }
}
