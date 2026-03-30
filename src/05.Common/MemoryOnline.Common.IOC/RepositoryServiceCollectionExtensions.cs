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
        public static IServiceCollection AddGenericRepositoryConfiguration(this IServiceCollection services)
        {

            // Registrar la interfaz para que se inyecte el mismo contexto
            services.AddScoped<IMyDbContext>(provider =>
                provider.GetRequiredService<MyDbContext>());

            services.AddDbContext<MyDbContext>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryEF<>));
            services.AddScoped(typeof(IGenericRepositoryRead<>), typeof(GenericRepositoryEFRead<>));
            services.AddScoped(typeof(IGenericRepositoryWrite<>), typeof(GenericRepositoryEFWrite<>));

            return services;
        }

        public static IServiceCollection AddGenericRepositorySqlServerConfiguration(this IServiceCollection services)
        {

            // Registrar la interfaz para que se inyecte el mismo contexto
            services.AddScoped<IMyDbContext>(provider =>
                provider.GetRequiredService<MyDbContext>());

            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer("Server=127.0.0.1,1433;Database=GameDb;User Id=sa;Password=TuPasswordFuerte123!;TrustServerCertificate=True;");
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryEF<>));
            services.AddScoped(typeof(IGenericRepositoryRead<>), typeof(GenericRepositoryEFRead<>));
            services.AddScoped(typeof(IGenericRepositoryWrite<>), typeof(GenericRepositoryEFWrite<>));

            return services;
        }
    }
}
