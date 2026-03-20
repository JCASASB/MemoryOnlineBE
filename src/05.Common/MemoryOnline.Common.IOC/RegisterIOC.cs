

using MemoryOnline.Domain.Domain.GameUseCases;
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Repository.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryOnline.Common.IOC
{
    public static class RegisterIOC
    {
        public static IServiceCollection AddDependencyInjectionForApplication(this IServiceCollection services)
        {
            // Repositorio y contexto EF Core InMemory
            services.AddGameRepositoryInMemory();

            services.AddScoped<IJoinGameUseCase, JoinGameUseCase>();
            services.AddScoped<ICreateGameUseCase, CreateGameUseCase>();
            services.AddScoped<IUpdateStateUseCase, UpdateStateUseCase>();
            // Capa de aplicación
            return services;
        }
    }
}
