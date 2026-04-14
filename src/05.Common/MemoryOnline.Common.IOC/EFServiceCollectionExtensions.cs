using MemoryOnline.Infraestructure.EF.Game.Context;
using MemoryOnline.Infraestructure.EF.Game.Repositories;
using MemoryOnline.Infraestructure.Generic;
using MemoryOnline.Infraestructure.IRepository;
using MemoryOnline.Infraestructure.Repository;
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

        public static IServiceCollection AddEFMongoDB(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<AppGameDbContexMongoDB>());

            services.AddDbContext<AppGameDbContexMongoDB>();

            services.AddScoped<IMatchRepository, MatchRepositoryEF>();

            return services;
        }
    }
    
}
