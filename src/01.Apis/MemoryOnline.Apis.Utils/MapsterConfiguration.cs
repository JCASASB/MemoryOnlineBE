using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryOnline.Apis.Utils
{
    public static class MapsterConfiguration
    {
        public static void AddMapsterConfig(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;

            // Escanea el ensamblado actual en busca de clases que implementen IRegister
            config.Scan(Assembly.GetExecutingAssembly());

            // Registramos la configuración como Singleton
            services.AddSingleton(config);

            // Registramos el Mapper como Scoped (recomendado para inyectar en Controladores/Hubs)
            services.AddScoped<IMapper, ServiceMapper>();
        }
    }
}
