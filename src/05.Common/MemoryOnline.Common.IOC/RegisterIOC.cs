using MemoryOnline.Domain.Domain.GameUseCases;
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Domain.Domain.IMatchUseCases;
using MemoryOnline.Domain.Domain.MatchUseCases;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryOnline.Common.IOC
{
    public static class RegisterIOC
    {
        public static IServiceCollection AddDependencyInjectionForApplication(this IServiceCollection services)
        {
            // Repositorio y contexto EF Core InMemory
           // services.AddGenericRepositoryConfiguration();

            // InMemory:
            //services.AddEFSqlServer();
            services.AddEFInMemory();
            //services.AddEFMongoDB();

            services.AddEFUsers();
            services.AddGenericDIConfiguration();

            //services.AddAppRepositoryInMemory();
            //services.AddAppRepositorySqlServer();

            services.AddScoped<ICreateMatchUseCase, CreateMatchUseCase>();
            services.AddScoped<IAddNewStateUseCase, AddNewStateUseCase>();

            // Capa de aplicación
           // services.AddScoped<UsersApplication>();
            return services;
        }


        public static IServiceCollection AddDependencyInjectionForWebApi(this IServiceCollection services)
        {
            services.AddEFUsers();
            services.AddGenericDIConfiguration();

            return services;
        }
    }
}
