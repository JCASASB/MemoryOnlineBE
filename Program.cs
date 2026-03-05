using MemoryOnlineBE.Hubs;

var builder = WebApplication.CreateBuilder(args);

// SignalR
builder.Services.AddSignalR();

// CORS - permitir el frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>()
            ?? Array.Empty<string>();

        var defaultOrigins = new[]
        {
            "http://localhost:5173",  // Vite dev server
            "http://localhost:4173"   // Vite preview
        };

        policy.WithOrigins(defaultOrigins.Concat(allowedOrigins).ToArray())
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
