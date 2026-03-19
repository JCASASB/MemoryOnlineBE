using MemoryOnline.Apis.Signalr;
using MemoryOnline.Apis.Signalr.Hubs;
using MemoryOnline.Application.Application.Commands.JoinGame;
using MemoryOnline.Application.Application.Commands.UpdateGameState;
using MemoryOnline.Common.IOC;
using Microsoft.AspNetCore.SignalR;
using MemoryOnline.Apis.Utils;

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
    });

// Registrar dependencias centralizadas (IOC)
builder.Services.AddDependencyInjectionForApplication();

// Registrar MediatR y handlers
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<JoinGameHandler>();
    cfg.RegisterServicesFromAssemblyContaining<UpdateGameStateHandler>();
});

// Registrar configuración de Mapster, mapeo de dtos
builder.Services.AddMapsterConfig();

var app = builder.Build();

// SignalR hubs
app.UseCors();
app.MapHub<GameHub>("/gamehub");

app.Run();