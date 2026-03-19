using MemoryOnline.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryOnline.Repository.Repository
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddGameRepositoryInMemory(this IServiceCollection services, string dbName = "GameDb")
        {
            services.AddDbContext<GameDbContext>(options =>
                options.UseInMemoryDatabase(dbName));
            services.AddScoped<IRepositoryGame, GameRepository>();
            return services;
        }
    }
}
