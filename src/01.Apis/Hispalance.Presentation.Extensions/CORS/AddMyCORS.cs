using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hispalance.Presentation.Extensions.CORS
{
    /*
     * para produccion cambiar a este:
     * policy.WithOrigins("https://tudominio.com", "https://otro.com")
      .AllowAnyMethod()
      .AllowAnyHeader();

    */
    public static class AddMyCORS
    {
        public static IServiceCollection AddMyCORSAddAll(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return services;
        }
    }



}
