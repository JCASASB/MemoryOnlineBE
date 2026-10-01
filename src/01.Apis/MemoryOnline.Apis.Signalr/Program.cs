using Hispalance.Presentation.Extensions.AutoriAuthori;
using MemoryOnline.Apis.Signalr;
using MemoryOnline.Apis.Signalr.Hubs;
using MemoryOnline.Apis.Utils;
using MemoryOnline.Application.Game.GameAppplication.Commands.CreateMatch;
using MemoryOnline.Common.IOC;
using MemoryOnline.Infraestructure.IRepository;
using Microsoft.Azure.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(_ => true) // <--- ESTO permite CUALQUIER origen
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Necesario para SignalR
    });
});

#region SignalR Configuration

//mi identificador personalizado para SignalR, que se basa en el nombre de usuario del contexto de autenticación
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
// Registrar SignalR y filtros globales
var signalRBuilder = builder.Services.AddSignalR(options =>
    {
        options.AddFilter<GlobalHubExceptionFilter>();
        options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    })
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
        options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// En desarrollo el cliente conecta directamente al hub local. En Azure se usa
// el servicio administrado mediante Azure__SignalR__ConnectionString.
if (!builder.Environment.IsDevelopment())
{
    signalRBuilder.AddAzureSignalR();
}

// Usa la extensión completa con JWT configurado desde appsettings
builder.Services.AddAutentiAuthoriForServices(builder.Configuration);

// Para SignalR: leer el token desde query string access_token
builder.Services.Configure<JwtBearerOptions>(
    JwtBearerDefaults.AuthenticationScheme,
    options =>
    {
        var existingOnMessageReceived = options.Events?.OnMessageReceived;
        options.Events ??= new JwtBearerEvents();
        options.Events.OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/gamehub"))
            {
                context.Token = accessToken;
            }
            return existingOnMessageReceived != null
                ? existingOnMessageReceived(context)
                : Task.CompletedTask;
        };
    });

# endregion

// Registrar dependencias centralizadas (IOC)
builder.Services.AddDependencyInjectionForGame(builder.Configuration);
builder.Services.AddScoped<IHubDomainEvents, HubDomainEvents>();

// Registrar MediatR y handlers
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateMatchHandler>(); 
});

// Registrar configuración de Mapster, mapeo de dtos
builder.Services.AddMapsterConfig();

#region OpenTelemetry
var otelEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];

if (!string.IsNullOrWhiteSpace(otelEndpoint))
{
    const string serviceName = "memoryonline-signalr";
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

// SignalR hubs
app.UseCors();
app.AddAuthoriAuthoriForApplication(); // UseAuthentication + UseAuthorization
app.MapHub<HubApplication>("/gamehub");

app.Run();
