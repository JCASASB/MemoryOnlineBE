using Hispalance.Infraestructure.DB.IRepositories.Generic;
using Hispalance.Infraestructure.DB.Repositories.EF;
using MemoryOnline.Infraestructure.EF.Application.Context;
using MemoryOnline.Infraestructure.EF.Application.Repositories;
using MemoryOnline.Infraestructure.EF.Game.Context;
using MemoryOnline.Infraestructure.EF.Game.Repositories;
using MemoryOnline.Infraestructure.IRepository.Application;
using MemoryOnline.Infraestructure.IRepository.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryOnline.Common.IOC
{
    public static class EFServiceCollectionExtensions
    {
        /// <summary>
        /// Registra ApplicationDbContext con InMemory y IGameRepository
        /// </summary>
        public static IServiceCollection AddEFInMemory(this IServiceCollection services)
        {
            services.AddScoped<IGameDbContext>(provider =>
                provider.GetRequiredService<GameDbContextInMemory>());

            services.AddDbContext<GameDbContextInMemory>();

            services.AddScoped<IGameRepository, GameRepositoryEF>();

            return services;
        }

        /// <summary>
        /// Registra ApplicationDbContext con SQL Server y IGameRepository
        /// </summary>
        public static IServiceCollection AddEFSqlServer(this IServiceCollection services)
        {
            services.AddScoped<IGameDbContext>(provider =>
                provider.GetRequiredService<GameDbContextSqlServer>());

            services.AddDbContext<GameDbContextSqlServer>();

            services.AddScoped<IGameRepository, GameRepositoryEF>();

            return services;
        }

        public static IServiceCollection AddEFMongoDB(this IServiceCollection services)
        {
            services.AddScoped<IGameDbContext>(provider =>
                provider.GetRequiredService<GameDbContexMongoDB>());

            services.AddDbContext<GameDbContexMongoDB>();

            services.AddScoped<IGameRepository, GameRepositoryEF>();

            return services;
        }

        public static IServiceCollection AddEFUsers(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();

            // 1. Registra la implementación concreta para su interfaz específica.
            services.AddScoped<IApplicationUOW, ApplicationUOW>();

            return services;
        }



        
        public static void AddGenericDIConfiguration(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(UsersGenericRepositoryEF<>));
            services.AddScoped(typeof(IGenericRepositoryRead<>), typeof(UsersGenericRepositoryEFRead<>));
            services.AddScoped(typeof(IGenericRepositoryWrite<>), typeof(UsersGenericRepositoryEFWrite<>));
        }
    }

    internal class UsersGenericRepositoryEF<TEntity> : GenericRepositoryEF<TEntity> where TEntity : class
    {
        public UsersGenericRepositoryEF(ApplicationDbContext context) : base(context) { }
    }

    internal class UsersGenericRepositoryEFRead<TEntity> : GenericRepositoryEFRead<TEntity> where TEntity : class
    {
        public UsersGenericRepositoryEFRead(ApplicationDbContext context) : base(context) { }
    }

    internal class UsersGenericRepositoryEFWrite<TEntity> : GenericRepositoryEFWrite<TEntity> where TEntity : class
    {
        public UsersGenericRepositoryEFWrite(ApplicationDbContext context) : base(context) { }
    }
}
