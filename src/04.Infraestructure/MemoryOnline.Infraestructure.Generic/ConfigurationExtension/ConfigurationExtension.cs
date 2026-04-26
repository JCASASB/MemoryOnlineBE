using Microsoft.Extensions.Configuration;

namespace MemoryOnline.Infraestructure.Generic.ConfigurationExtension
{
    public static class ConfigurationExtensions
    {
        public static string MyGetConnectionString(this IConfiguration config)
        {
            try
            {
                var server = config.GetSection("DBSection:server").Value;
                var port = config.GetSection("DBSection:port").Value;
                var database = config.GetSection("DBSection:database").Value;
                var user = config.GetSection("DBSection:user").Value;
                var pass = config.GetSection("DBSection:pass").Value;

                // Devuelve la cadena formateada
                return $"server={server};Port={port};database={database};uid={user};pwd={pass}";
            }
            catch (Exception ex)
            {
                throw new Exception("Algo falla al recuperar los datos " +
                    "de la conection string en el dbcontext", ex);
            }

        }
    }
}
