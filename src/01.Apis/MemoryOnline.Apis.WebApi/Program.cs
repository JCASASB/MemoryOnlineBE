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
builder.Services.AddDependencyInjectionForWebApi();

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
/*
var otelEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] != null
    ? new Uri(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]!)
    : new Uri("http://otel-collector:4317"); // Nombre del servicio en Docker

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(
            serviceName: "MemoryOnline.WebApi", // Pon un nombre descriptivo
            serviceVersion: "1.0.0"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSqlClientInstrumentation() // ¡Añade esto para ver consultas a SQL!
        .AddOtlpExporter(options => options.Endpoint = otelEndpoint))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation() // Métricas de CPU/RAM de .NET
        .AddOtlpExporter(options => options.Endpoint = otelEndpoint));

// Configuración específica para LOGS
/*builder.Logging.ClearProviders(); // Opcional: Limpiar logs por defecto si solo quieres enviar a OTLP
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true; // Importante para correlación
    logging.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MemoryOnline.WebApi")); // Importante para que Loki identifique el servicio
    logging.AddOtlpExporter(options => options.Endpoint = otelEndpoint);
});*/
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