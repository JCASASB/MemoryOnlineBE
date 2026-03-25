using MemoryOnline.Domain.Entities;
using System.Collections.Generic;

namespace MemoryOnline.Domain.Domain.UserUseCases
{
    public class ListUsersUseCase
    {
        public IEnumerable<Usuario> Execute(IEnumerable<Usuario> users)
        {
            // Devuelve la lista de usuarios (puedes agregar paginación o filtros)
            return users;
        }
    }
}
