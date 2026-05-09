using LibraryApp.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Database.DbContext;

public class LibraryDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).IsRequired().HasMaxLength(255);
            entity.Property(b => b.Author).IsRequired().HasMaxLength(255);
            entity.Property(b => b.Isbn).HasMaxLength(20);
            entity.Property(b => b.Owner).IsRequired().HasMaxLength(255);
            entity.Property(b => b.IsAvailable).HasDefaultValue(true);
        });

        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", Isbn = "978-0132350884", PublishedDate = new DateOnly(2008, 8, 1), Owner = "Alice", IsAvailable = true },
            new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "David Thomas", Isbn = "978-0135957059", PublishedDate = new DateOnly(2019, 9, 23), Owner = "Bob", IsAvailable = false },
            new Book { Id = 3, Title = "Design Patterns", Author = "Gang of Four", Isbn = "978-0201633610", PublishedDate = new DateOnly(1994, 10, 31), Owner = "Alice", IsAvailable = true },
            new Book { Id = 4, Title = "Refactoring", Author = "Martin Fowler", Isbn = "978-0134757599", PublishedDate = new DateOnly(2018, 11, 20), Owner = "Carol", IsAvailable = true },
            new Book { Id = 5, Title = "Domain-Driven Design", Author = "Eric Evans", Isbn = "978-0321125217", PublishedDate = new DateOnly(2003, 8, 22), Owner = "Bob", IsAvailable = false }
        );
    }
}
