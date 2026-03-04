using MemoryOnlineBE.Hubs;

var builder = WebApplication.CreateBuilder(args);

// SignalR
builder.Services.AddSignalR();

// CORS - permitir el frontend de Vite
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",  // Vite dev server
                "http://localhost:4173"   // Vite preview
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Requerido por SignalR
    });
});

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "MemoryOnline API is running");
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.MapHub<GameHub>("/gamehub");

app.Run();
