using MemoryOnline.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MemoryOnline.Domain.Entities.UserUseCases
{
    public class GetUserByNameUseCase
    {
        public Usuario Execute(IEnumerable<Usuario> users, string name)
        {
            // Busca un usuario por nombre (case-insensitive)
            return users.FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
