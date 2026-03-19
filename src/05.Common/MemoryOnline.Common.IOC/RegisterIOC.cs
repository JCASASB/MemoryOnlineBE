

using Microsoft.Extensions.DependencyInjection;
using MemoryOnline.Repository.Repository;

namespace MemoryOnline.Common.IOC
{
    public static class RegisterIOC
    {
        public static IServiceCollection AddDependencyInjectionForApplication(this IServiceCollection services)
        {
            // Repositorio y contexto EF Core InMemory
            services.AddGameRepositoryInMemory();
            // Capa de aplicación
            return services;
        }
    }
}
