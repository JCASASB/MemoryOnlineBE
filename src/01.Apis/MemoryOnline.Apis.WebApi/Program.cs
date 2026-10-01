using Hispalance.Presentation.Extensions.AutoriAuthori;
using Hispalance.Presentation.Extensions.CORS;
using Hispalance.Presentation.Extensions.OpenApiScalarExt;
using MemoryOnline.Application.Users.UsersApplication.Queries.GetAllUsers;
using MemoryOnline.Common.IOC;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencyInjectionForWebApi(builder.Configuration);

// CORS - Permitir los orígenes de la configuración
builder.Services.AddMyCORSAddOrigins(builder.Configuration);

builder.Services.AddControllersWithViews(); // Suport per a MVC o API

//From My Extensions
builder.Services.AddOpenApiScalarForServices();

//Add My Extensions for auth aut
builder.Services.AddAutentiAuthoriForServices(builder.Configuration);

// Registrar MediatR y handlers
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetAllUsersHandler>();
});


#region OpenTelemetry
var otelEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];

if (!string.IsNullOrWhiteSpace(otelEndpoint))
{
    const string serviceName = "memoryonline-api";
    var resource = ResourceBuilder.CreateDefault()
        .AddService(serviceName)
        .AddAttributes([
            new KeyValuePair<string, object>("service.namespace", "memoryonline"),
            new KeyValuePair<string, object>(
                "deployment.environment.name",
                builder.Environment.EnvironmentName)
        ]);

    builder.Services.AddOpenTelemetry()
        .ConfigureResource(resourceBuilder => resourceBuilder
            .AddService(serviceName)
            .AddAttributes([
                new KeyValuePair<string, object>("service.namespace", "memoryonline"),
                new KeyValuePair<string, object>(
                    "deployment.environment.name",
                    builder.Environment.EnvironmentName)
            ]))
        .WithTracing(tracing => tracing
            // Se conserva solo una muestra de las peticiones correctas para proteger la cuota gratuita.
            .SetSampler(new ParentBasedSampler(new TraceIdRatioBasedSampler(0.05)))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter())
        .WithMetrics(metrics => metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddOtlpExporter());

    builder.Logging.AddOpenTelemetry(logging =>
    {
        logging.IncludeFormattedMessage = true;
        logging.IncludeScopes = true;
        logging.SetResourceBuilder(resource);
        logging.AddOtlpExporter();
    });
}
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline. 

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
   
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// CORS - Aplicar política
app.UseCors("AllowSpecificOrigins");

//From My Extensions
app.AddOpenApiScalarForApplication();

//From My Extensions for auth aut
app.AddAuthoriAuthoriForApplication();

app.MapControllers();

app.MapGet("/", () => "Hola món des de Minimal APIs!"); // Exemple de Minimal API

app.Run();
