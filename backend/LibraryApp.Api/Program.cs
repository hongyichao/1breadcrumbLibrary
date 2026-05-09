using LibraryApp.Api.Middleware;
using LibraryApp.Api.Repositories;
using LibraryApp.Database.DbContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));

builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors();
app.MapControllers();

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    db.Database.Migrate();
}

app.Run();

public partial class Program { }
