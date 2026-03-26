using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Domain.Domain.UserUseCases
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
