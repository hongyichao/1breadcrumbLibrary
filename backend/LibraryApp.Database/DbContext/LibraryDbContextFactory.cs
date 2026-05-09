using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibraryApp.Database.DbContext;

public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
{
    public LibraryDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseMySql(
                "Server=localhost;Port=3306;Database=library_db;User=library_user;Password=library_password;",
                new MySqlServerVersion(new Version(8, 0, 0)))
            .Options;

        return new LibraryDbContext(options);
    }
}
