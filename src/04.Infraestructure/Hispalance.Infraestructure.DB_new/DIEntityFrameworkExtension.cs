
using Hispalance.Infraestructure.DB.IRepositories.Generic;
using Hispalance.Infraestructure.DB.Repositories.EF;
using Microsoft.Extensions.DependencyInjection;

namespace Hispalance.Infraestructure.DB
{
   public static class DIEntityFrameworkExtension
    {
        public static void AddGenericDFConfiguration(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryEF<>));
            services.AddScoped(typeof(IGenericRepositoryRead<>), typeof(GenericRepositoryEFRead<>));
            services.AddScoped(typeof(IGenericRepositoryWrite<>), typeof(GenericRepositoryEFWrite<>));
        }
    }
}
