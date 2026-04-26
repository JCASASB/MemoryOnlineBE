using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Domain.Domain.Specifications.Implementations
{
    public class UserFilterByNameSpec : BaseSpecification<Usuario>
    {
        public UserFilterByNameSpec(string nameUser)
            : base(u => u.Name == nameUser)
        {
            // Regla: Siempre traer la categoría relacionada
            //AddInclude(p => p.Category);
        }
    }
}
