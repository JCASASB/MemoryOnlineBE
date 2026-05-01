using MemoryOnline.Domain.Entities.Users;

namespace MemoryOnline.Domain.Domain.Specifications.Implementations
{
    public class UserResultsByUserIdSpec : BaseSpecification<Usuario>
    {
        public UserResultsByUserIdSpec(Guid userId)
            : base(u => u.Id == userId)
        {
            // Regla: Siempre traer la categoría relacionada
            AddInclude(u => u.Results);
        }
    }
}
