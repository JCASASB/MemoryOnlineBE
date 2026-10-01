using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Domain.Entities.Users;

namespace MemoryOnline.Domain.Domain.Specifications.Implementations
{
    public class UserResultsFilterByUserIdSpec : BaseSpecification<UserMatchResult>
    {
        public UserResultsFilterByUserIdSpec(Guid userId)
            : base(u => u.UsuarioId == userId)
        {
            // Regla: Siempre traer la categoría relacionada
            //AddInclude(u => u.Results);
        }
    }
}
