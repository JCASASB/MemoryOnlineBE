using MemoryOnline.Infraestructure.EF.Context;
using MemoryOnline.Infraestructure.EF.Repositories;
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
                provider.GetRequiredService<ApplicationDbContextInMemory>());

            services.AddDbContext<ApplicationDbContextInMemory>();

            services.AddScoped<IMatchRepository, MatchRepositoryEF>();

            return services;
        }

        /// <summary>
        /// Registra ApplicationDbContext con SQL Server y IGameRepository
        /// </summary>
        public static IServiceCollection AddEFSqlServer(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContextSqlServer>());

            services.AddDbContext<ApplicationDbContextSqlServer>();

            services.AddScoped<IMatchRepository, MatchRepositoryEF>();

            return services;
        }

        public static IServiceCollection AddEFMongoDB(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContextMongoDB>());

            services.AddDbContext<ApplicationDbContextMongoDB>();

            services.AddScoped<IMatchRepository, MatchRepositoryEF>();

            return services;
        }
    }
    
}
