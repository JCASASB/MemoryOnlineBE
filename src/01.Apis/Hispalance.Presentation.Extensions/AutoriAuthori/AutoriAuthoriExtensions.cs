using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Hispalance.Presentation.Extensions.AutoriAuthori
{
    /*
     *  Add in the program as 
     *  //Add My Extensions for auth aut
        builder.Services.AddAuthoriAuthoriForServices(builder.Configuration);
     * 
     * also add config in appsettings.json
     * "JwtSettings": {
            "Key": "0123456789abcdef0123456789abcdef",
            "Issuer": "TheApi",
            "Audience": "TheAppCliente"
          },
     */
    public static class OpenApiScalarExtensions
    {
        public static IServiceCollection AddAutentiAuthoriForServices(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtKey = configuration["JwtSettings:Key"];

            // Validación básica para evitar errores al arrancar
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("La JWT SecretKey no está configurada.");


            services
                .AddAuthorization()
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        public static WebApplication AddAuthoriAuthoriForApplication(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
