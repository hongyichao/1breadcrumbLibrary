using LibraryApp.Database.Entities;

namespace LibraryApp.Api.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync(string? search, bool? isAvailable);
    Task<Book?> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book book);
    Task<Book?> UpdateAsync(int id, Book book);
    Task<bool> DeleteAsync(int id);
    Task<Book?> ToggleAvailabilityAsync(int id);
}
