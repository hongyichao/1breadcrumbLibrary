using LibraryApp.Database.DbContext;
using LibraryApp.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Api.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync(string? search, bool? isAvailable)
    {
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(b => b.Title.ToLower().Contains(search.ToLower()));

        if (isAvailable.HasValue)
            query = query.Where(b => b.IsAvailable == isAvailable.Value);

        return await query.OrderBy(b => b.Title).ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id) =>
        await _context.Books.FindAsync(id);

    public async Task<Book> CreateAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book?> UpdateAsync(int id, Book updated)
    {
        var book = await _context.Books.FindAsync(id);
        if (book is null) return null;

        book.Title = updated.Title;
        book.Author = updated.Author;
        book.Isbn = updated.Isbn;
        book.PublishedDate = updated.PublishedDate;
        book.Owner = updated.Owner;
        book.IsAvailable = updated.IsAvailable;

        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book is null) return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Book?> ToggleAvailabilityAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book is null) return null;

        book.IsAvailable = !book.IsAvailable;
        await _context.SaveChangesAsync();
        return book;
    }
}
