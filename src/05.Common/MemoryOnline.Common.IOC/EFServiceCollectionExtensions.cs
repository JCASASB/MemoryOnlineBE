using Hispalance.Infraestructure.DB.IRepositories.Generic;
using Hispalance.Infraestructure.DB.Repositories.EF;
using MemoryOnline.Infraestructure.EF.Game.Context;
using MemoryOnline.Infraestructure.EF.Game.Repositories;
using MemoryOnline.Infraestructure.EF.Users;
using MemoryOnline.Infraestructure.EF.Users.Context;
using MemoryOnline.Infraestructure.IRepository;
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
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<AppGameDbContextInMemory>());

            services.AddDbContext<AppGameDbContextInMemory>();

            services.AddScoped<IMatchRepository, MatchRepositoryEF>();

            return services;
        }

        /// <summary>
        /// Registra ApplicationDbContext con SQL Server y IGameRepository
        /// </summary>
        public static IServiceCollection AddEFSqlServer(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<AppGameDbContextSqlServer>());

            services.AddDbContext<AppGameDbContextSqlServer>();

            services.AddScoped<IMatchRepository, MatchRepositoryEF>();

            return services;
        }

        public static IServiceCollection AddEFMongoDB(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<AppGameDbContexMongoDB>());

            services.AddDbContext<AppGameDbContexMongoDB>();

            services.AddScoped<IMatchRepository, MatchRepositoryEF>();

            return services;
        }

        public static IServiceCollection AddEFUsers(this IServiceCollection services)
        {
            services.AddDbContext<UsersDbContext>();

            // Añada esta línea para mapear DbContext a su implementación concreta
            services.AddScoped<DbContext>(provider => provider.GetRequiredService<UsersDbContext>());

            // 1. Registra la implementación concreta para su interfaz específica.
            services.AddScoped<IUsersRepository, UsersRepository>();

            return services;
        }



        
        public static void AddGenericDIConfiguration(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryEF<>));
            services.AddScoped(typeof(IGenericRepositoryRead<>), typeof(GenericRepositoryEFRead<>));
            services.AddScoped(typeof(IGenericRepositoryWrite<>), typeof(GenericRepositoryEFWrite<>));
        }
    }
    
}
