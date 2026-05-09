using LibraryApp.Api.Models;
using LibraryApp.Api.Repositories;
using LibraryApp.Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _repository;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IBookRepository repository, ILogger<BooksController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks(
        [FromQuery] string? search,
        [FromQuery] bool? isAvailable)
    {
        _logger.LogInformation("Fetching books with search={Search}, isAvailable={IsAvailable}", search, isAvailable);
        var books = await _repository.GetAllAsync(search, isAvailable);
        return Ok(books.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDto>> GetBook(int id)
    {
        var book = await _repository.GetByIdAsync(id);
        if (book is null)
        {
            _logger.LogWarning("Book {Id} not found", id);
            return NotFound();
        }
        return Ok(ToDto(book));
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookRequest request)
    {
        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            Isbn = request.Isbn,
            PublishedDate = request.PublishedDate,
            Owner = request.Owner,
            IsAvailable = true
        };

        var created = await _repository.CreateAsync(book);
        _logger.LogInformation("Created book {Id} - {Title}", created.Id, created.Title);
        return CreatedAtAction(nameof(GetBook), new { id = created.Id }, ToDto(created));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookDto>> UpdateBook(int id, [FromBody] UpdateBookRequest request)
    {
        var updated = await _repository.UpdateAsync(id, new Book
        {
            Title = request.Title,
            Author = request.Author,
            Isbn = request.Isbn,
            PublishedDate = request.PublishedDate,
            Owner = request.Owner,
            IsAvailable = request.IsAvailable
        });

        if (updated is null)
        {
            _logger.LogWarning("Update failed — book {Id} not found", id);
            return NotFound();
        }

        _logger.LogInformation("Updated book {Id}", id);
        return Ok(ToDto(updated));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("Delete failed — book {Id} not found", id);
            return NotFound();
        }

        _logger.LogInformation("Deleted book {Id}", id);
        return NoContent();
    }

    [HttpPatch("{id:int}/availability")]
    public async Task<ActionResult<BookDto>> ToggleAvailability(int id)
    {
        var book = await _repository.ToggleAvailabilityAsync(id);
        if (book is null)
        {
            _logger.LogWarning("Toggle failed — book {Id} not found", id);
            return NotFound();
        }

        _logger.LogInformation("Toggled availability for book {Id} — now {IsAvailable}", id, book.IsAvailable);
        return Ok(ToDto(book));
    }

    private static BookDto ToDto(Book b) =>
        new(b.Id, b.Title, b.Author, b.Isbn, b.PublishedDate, b.Owner, b.IsAvailable);
}
