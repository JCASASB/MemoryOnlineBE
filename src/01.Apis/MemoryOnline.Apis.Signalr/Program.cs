using MemoryOnline.Apis.Signalr;
using MemoryOnline.Apis.Signalr.Hubs;
using MemoryOnline.Apis.Utils;
using MemoryOnline.Application.Application.GameAppplication.Commands.JoinGame;
using MemoryOnline.Common.IOC;
using Microsoft.AspNetCore.SignalR;
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

// Registrar SignalR y filtros globales
builder.Services.AddSignalR(options =>
    {
        options.AddFilter<GlobalHubExceptionFilter>();
    })
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
        options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.PayloadSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// Registrar dependencias centralizadas (IOC)
builder.Services.AddDependencyInjectionForApplication();

// Registrar MediatR y handlers
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<JoinGameHandler>();
});

// Registrar configuración de Mapster, mapeo de dtos
builder.Services.AddMapsterConfig();

var app = builder.Build();

// SignalR hubs
app.UseCors();
app.MapHub<GameHub>("/gamehub");

app.Run();