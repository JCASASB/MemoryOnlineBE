using MemoryOnline.Common.IOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencyInjectionForApplication();


builder.Services.AddControllersWithViews(); // Suport per a MVC o API
builder.Services.AddEndpointsApiExplorer(); // Necessari per a Swagger
builder.Services.AddSwaggerGen();           // Genera documentació API


var app = builder.Build();

// Configure the HTTP request pipeline. 

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MemoryOnline API V1");
        options.RoutePrefix = string.Empty; // Swagger at app root
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();

app.MapGet("/", () => "Hola món des de Minimal APIs!"); // Exemple de Minimal API

app.Run();