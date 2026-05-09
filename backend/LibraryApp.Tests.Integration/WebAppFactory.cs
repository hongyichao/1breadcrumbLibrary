using LibraryApp.Database.DbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryApp.Tests.Integration;

public class WebAppFactory : WebApplicationFactory<Program>
{
    // Keep one open connection alive so the in-memory SQLite DB persists between scopes
    private readonly SqliteConnection _keepAliveConnection = new("DataSource=:memory:");

    public WebAppFactory()
    {
        _keepAliveConnection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<LibraryDbContext>));
            if (descriptor is not null) services.Remove(descriptor);

            services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlite(_keepAliveConnection));
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
        db.Database.EnsureCreated();

        return host;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing) _keepAliveConnection.Dispose();
    }
}
