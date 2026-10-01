using Hispalance.Infraestructure.DB.IRepositories.Generic;
using Hispalance.Infraestructure.DB.Repositories.EF;
using MemoryOnline.Infraestructure.EF.Application.Repositories;
using MemoryOnline.Infraestructure.EF.Game.Context;
using MemoryOnline.Infraestructure.EF.Game.Repositories;
using MemoryOnline.Infraestructure.IRepository.Application;
using MemoryOnline.Infraestructure.IRepository.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryOnline.Common.IOC
{
    public static class EFServiceCollectionExtensions
    {
        /// <summary>
        /// Registra ApplicationDbContext con InMemory y IGameRepository
        /// </summary>
        public static IServiceCollection AddGameRepositoryEF(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddConfiguredGameDbContext(configuration);
            services.AddScoped<IGameRepository, GameRepositoryEF>();

            return services;
        }


        public static IServiceCollection AddUsersRepositoryEF(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddConfiguredGameDbContext(configuration);
            services.AddGenericDIConfiguration();

            services.AddScoped<IApplicationUOW, ApplicationUOW>();

            return services;
        }

        public static void AddGenericDIConfiguration(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryEF<>));
            services.AddScoped(typeof(IGenericRepositoryRead<>), typeof(GenericRepositoryEFRead<>));
            services.AddScoped(typeof(IGenericRepositoryWrite<>), typeof(GenericRepositoryEFWrite<>));
        }

        private static IServiceCollection AddConfiguredGameDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var provider = configuration["DBSection:Provider"];

            if (string.Equals(provider, "MongoDB", StringComparison.OrdinalIgnoreCase))
            {
                services.AddDbContext<GameDbContextMongo>();
                services.AddScoped<IGameDbContext>(sp => sp.GetRequiredService<GameDbContextMongo>());
                services.AddScoped<DbContext>(sp => sp.GetRequiredService<GameDbContextMongo>());
            }
            else
            {
                services.AddDbContext<GameDbContext>();
                services.AddScoped<IGameDbContext>(sp => sp.GetRequiredService<GameDbContext>());
                services.AddScoped<DbContext>(sp => sp.GetRequiredService<GameDbContext>());
            }

            return services;
        }
    }
}
