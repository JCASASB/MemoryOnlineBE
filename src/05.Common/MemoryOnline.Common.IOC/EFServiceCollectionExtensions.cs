using MemoryOnline.Infraestructure.EF.Context;
using MemoryOnline.Infraestructure.EF.Repositories;
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
            services.AddDbContext<ApplicationDbContext>();

            services.AddScoped<IGameRepository, GameRepository>();

            return services;
        }

        /// <summary>
        /// Registra ApplicationDbContext con SQL Server y IGameRepository
        /// </summary>
        public static IServiceCollection AddEFSqlServer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IGameRepository, GameRepository>();

            return services;
        }
    }
}
