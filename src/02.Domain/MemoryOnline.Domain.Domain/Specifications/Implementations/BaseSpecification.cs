using MemoryOnline.Domain.Domain.Specifications.Interfaces;
using System.Linq.Expressions;

namespace MemoryOnline.Domain.Domain.Specifications.Implementations
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        // El filtro principal (Where)
        public Expression<Func<T, bool>> Criteria { get; }

        // Lista de relaciones a incluir (Includes)
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        // Propiedades para el ordenamiento
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        // Constructor que obliga a definir un filtro inicial
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        // Método para añadir Includes de forma sencilla
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        // Métodos para definir el orden (solo uno puede estar activo)
        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }
    }
}
