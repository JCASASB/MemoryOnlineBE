

using MemoryOnline.Domain.Domain.GameUseCases;
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Domain.Domain.IMatchUseCases;
using MemoryOnline.Domain.Domain.MatchUseCases;
using MemoryOnline.Infraestructure.IRepository;
using MemoryOnline.Repository.Repository;
using Microsoft.EntityFrameworkCore;
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
            //services.AddEFInMemory();
            services.AddEFMongoDB("");


            //services.AddAppRepositoryInMemory();
            //services.AddAppRepositorySqlServer();

            services.AddScoped<IJoinMatchUseCase, JoinMatchUseCase>();
            services.AddScoped<ICreateMatchUseCase, CreateMatchUseCase>();
            services.AddScoped<IAddNewStateUseCase, AddNewStateUseCase>();

            // Capa de aplicación
           // services.AddScoped<UsersApplication>();
            return services;
        }
    }
}
