using MemoryOnline.Domain.Domain.GameUseCases;
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Domain.Domain.IMatchUseCases;
using MemoryOnline.Domain.Domain.MatchUseCases;
using MemoryOnline.Domain.Domain.UserStatsUseCases;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryOnline.Common.IOC
{
    public static class RegisterIOC
    {
        /*
         * Las usadas por el signalr
         * */
        public static IServiceCollection AddDependencyInjectionForGame(this IServiceCollection services)
        {
            // Repositorio y contexto EF Core InMemory
            //services.AddEFSqlServer();
            //services.AddEFInMemory();
            services.AddEFMongoDB();

            services.AddEFUsers();
            services.AddGenericDIConfiguration();

            //services.AddAppRepositoryInMemory();
            //services.AddAppRepositorySqlServer();

            services.AddScoped<ICreateMatchUseCase, CreateMatchUseCase>();
            services.AddScoped<IAddNewStateUseCase, AddNewStateUseCase>();
            
            return services;
        }

        public static IServiceCollection AddDependencyInjectionForWebApi(this IServiceCollection services)
        {
            services.AddEFUsers();
            services.AddGenericDIConfiguration();

            services.AddScoped<IGetUserStatsByIdUserUseCase, GetUserStatsByIdUserUseCase>();

            return services;
        }
    }
}
