using MemoryOnline.Domain.Domain.ChallengeUseCases;
using MemoryOnline.Domain.Domain.GameUseCases;
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Domain.Domain.IMatchUseCases;
using MemoryOnline.Domain.Domain.MatchUseCases;
using MemoryOnline.Domain.Domain.UserStatsUseCases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Common.IOC
{
    public static class RegisterIOC
    {
        /*
         * Las usadas por el signalr
         * */
        public static IServiceCollection AddDependencyInjectionForGame(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Repositorio y contexto EF Core InMemory
            services.AddGameRepositoryEF(configuration);

            services.AddUsersRepositoryEF(configuration);

            services.AddScoped<ICreateMatchUseCase, CreateMatchUseCase>();
            services.AddScoped<IAddNewStateUseCase, AddNewStateUseCase>();
            services.AddScoped<ICreateChallengeUseCase, CreateChallengeUseCase>();

            return services;
        }

        public static IServiceCollection AddDependencyInjectionForWebApi(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddUsersRepositoryEF(configuration);

            services.AddScoped<IGetUserStatsByIdUserUseCase, GetUserStatsByIdUserUseCase>();

            return services;
        }
    }
}
