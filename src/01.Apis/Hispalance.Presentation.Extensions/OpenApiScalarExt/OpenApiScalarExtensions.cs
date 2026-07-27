using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Hispalance.Presentation.Extensions.OpenApiScalarExt
{
    public static class OpenApiScalarExtensions
    {
        public static IServiceCollection AddOpenApiScalarForServices(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            });

            return services;
        }

        public static WebApplication AddOpenApiScalarForApplication(this WebApplication app)
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "This is my Scalar API";
                options.DarkMode = true;
                options.Favicon = "path";
                options.DefaultHttpClient = new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.CSharp, ScalarClient.RestSharp);
                options.HideModels = false;
                options.Layout = ScalarLayout.Modern;
                options.ShowSidebar = true;

                options.Authentication = new ScalarAuthenticationOptions
                {
                    PreferredSecurityScheme = "Bearer"
                };
            });

            return app;
        }

    }
}
