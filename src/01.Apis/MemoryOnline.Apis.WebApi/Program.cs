using Hispalance.Presentation.Extensions.AutoriAuthori;
using Hispalance.Presentation.Extensions.OpenApiScalarExt;
using MemoryOnline.Application.Users.UsersApplication.Queries.GetAllUsers;
using MemoryOnline.Common.IOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencyInjectionForWebApi();

builder.Services.AddControllersWithViews(); // Suport per a MVC o API

//From My Extensions
builder.Services.AddOpenApiScalarForServices();

//Add My Extensions for auth aut
builder.Services.AddAuthoriAuthoriForServices(builder.Configuration);

// Registrar MediatR y handlers
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetAllUsersHandler>();
});


var app = builder.Build();

// Configure the HTTP request pipeline. 

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
   
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

//From My Extensions
app.AddOpenApiScalarForApplication();

//From My Extensions for auth aut
app.AddAuthoriAuthoriForApplication();

app.MapControllers();

app.MapGet("/", () => "Hola món des de Minimal APIs!"); // Exemple de Minimal API

app.Run();