using MemoryOnline.Domain.Entities.Users;

namespace MemoryOnline.Domain.Domain.Specifications.Implementations
{
    public class UserFilterByIdSpec : BaseSpecification<Usuario>
    {
        public UserFilterByIdSpec(Guid id)
            : base(u => u.Id == id)
        {
            // Regla: Siempre traer la categoría relacionada
            //AddInclude(p => p.Category);
        }
    }
}