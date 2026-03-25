

using MemoryOnline.Domain.Domain.GameUseCases;
using MemoryOnline.Domain.Domain.IGameUseCases;
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
            services.AddGameRepositoryInMemory();

            services.AddScoped<IJoinGameUseCase, JoinGameUseCase>();
            services.AddScoped<ICreateGameUseCase, CreateGameUseCase>();
            services.AddScoped<IUpdateStateUseCase, UpdateStateUseCase>();

            // Capa de aplicación
           // services.AddScoped<UsersApplication>();
            return services;
        }

        public static IServiceCollection AddGenericDIConfiguration(this IServiceCollection services)
        {

            services.AddScoped<IUsersRepository, UserRepository>();  

            services.AddDbContext<GameDbContext>(options =>
            {
                options.UseInMemoryDatabase("loquesea");
            });

            return services;
        }

        public static IServiceCollection AddGenericDI2Configuration(this IServiceCollection services)
        {
            //  services.AddScoped(typeof(IGenericRepositoryRead<>), typeof(GenericRepositoryEFRead<>));
            // services.AddScoped(typeof(IGenericRepositoryWrite<>), typeof(GenericRepositoryEFWrite<>));
            //services.AddScoped(typeof(IMyDbContext), typeof(MyDbContext));


            services.AddDbContext<GameDbContext>(options =>
            {
                options.UseInMemoryDatabase("loquesea");
            });

            return services;
        }
    }
}
