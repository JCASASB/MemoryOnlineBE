using MemoryOnline.Infraestructure.Generic;
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
using MemoryOnline.Infraestructure.Generic.Repositories.EF;
using MemoryOnline.Infraestructure.IRepository;
using MemoryOnline.Infraestructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryOnline.Repository.Repository
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddAppRepositoryInMemory(this IServiceCollection services, string dbName = "GameDb")
        {
            services.AddDbContext<GameDbContext>(options =>
                options.UseInMemoryDatabase(dbName));

            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IUsersRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddAppRepositorySqlServer(this IServiceCollection services)
        {

             services.AddDbContext<GameDbContext>(options =>
              {
                  // Obtiene la cadena de conexión desde appsettings.json
                  options.UseSqlServer("Server=127.0.0.1,1433;Database=GameDb;User Id=sa;Password=TuPasswordFuerte123!;TrustServerCertificate=True;");
              });

           // services.AddScoped<IUsersRepository, UserRepository>();
            services.AddScoped<IGameRepository, GameRepository>();

            return services;
        }

       


        public static IServiceCollection AddGenericRepositoryConfiguration(this IServiceCollection services)
        {

            // services.AddScoped(typeof(IMyDbContext), typeof(MyDbContext));

            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseInMemoryDatabase("loquesea");
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryEF<>));
            services.AddScoped(typeof(IGenericRepositoryRead<>), typeof(GenericRepositoryEFRead<>));
            services.AddScoped(typeof(IGenericRepositoryWrite<>), typeof(GenericRepositoryEFWrite<>));

            return services;
        }

    }
}
