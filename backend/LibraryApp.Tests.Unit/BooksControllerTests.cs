using LibraryApp.Api.Controllers;
using LibraryApp.Api.Models;
using LibraryApp.Api.Repositories;
using LibraryApp.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibraryApp.Tests.Unit;

public class BooksControllerTests
{
    private readonly Mock<IBookRepository> _repoMock;
    private readonly BooksController _controller;

    public BooksControllerTests()
    {
        _repoMock = new Mock<IBookRepository>();
        var logger = new Mock<ILogger<BooksController>>();
        _controller = new BooksController(_repoMock.Object, logger.Object);
    }

    [Fact]
    public async Task GetBooks_ReturnsOkWithAllBooks()
    {
        var books = new List<Book>
        {
            new() { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", Isbn = "123", PublishedDate = new DateOnly(2008, 1, 1), Owner = "Alice", IsAvailable = true },
            new() { Id = 2, Title = "Refactoring", Author = "Martin Fowler", Isbn = "456", PublishedDate = new DateOnly(2018, 1, 1), Owner = "Bob", IsAvailable = false }
        };
        _repoMock.Setup(r => r.GetAllAsync(null, null)).ReturnsAsync(books);

        var result = await _controller.GetBooks(null, null);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var dtos = Assert.IsAssignableFrom<IEnumerable<BookDto>>(ok.Value);
        Assert.Equal(2, dtos.Count());
    }

    [Fact]
    public async Task GetBook_WhenNotFound_ReturnsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Book?)null);

        var result = await _controller.GetBook(99);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetBook_WhenFound_ReturnsOkWithDto()
    {
        var book = new Book { Id = 1, Title = "Clean Code", Author = "R. Martin", Isbn = "123", PublishedDate = new DateOnly(2008, 1, 1), Owner = "Alice", IsAvailable = true };
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(book);

        var result = await _controller.GetBook(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<BookDto>(ok.Value);
        Assert.Equal("Clean Code", dto.Title);
        Assert.Equal("Alice", dto.Owner);
    }

    [Fact]
    public async Task CreateBook_ReturnsCreatedAtAction()
    {
        var request = new CreateBookRequest("New Book", "Author", "789", new DateOnly(2023, 1, 1), "Carol");
        var created = new Book { Id = 3, Title = "New Book", Author = "Author", Isbn = "789", PublishedDate = new DateOnly(2023, 1, 1), Owner = "Carol", IsAvailable = true };
        _repoMock.Setup(r => r.CreateAsync(It.IsAny<Book>())).ReturnsAsync(created);

        var result = await _controller.CreateBook(request);

        var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
        var dto = Assert.IsType<BookDto>(createdAt.Value);
        Assert.Equal(3, dto.Id);
        Assert.True(dto.IsAvailable);
    }

    [Fact]
    public async Task DeleteBook_WhenFound_ReturnsNoContent()
    {
        _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _controller.DeleteBook(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteBook_WhenNotFound_ReturnsNotFound()
    {
        _repoMock.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

        var result = await _controller.DeleteBook(99);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task ToggleAvailability_WhenFound_ReturnsUpdatedDto()
    {
        var book = new Book { Id = 1, Title = "Clean Code", Author = "R. Martin", Isbn = "123", PublishedDate = new DateOnly(2008, 1, 1), Owner = "Alice", IsAvailable = false };
        _repoMock.Setup(r => r.ToggleAvailabilityAsync(1)).ReturnsAsync(book);

        var result = await _controller.ToggleAvailability(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<BookDto>(ok.Value);
        Assert.False(dto.IsAvailable);
    }

    [Fact]
    public async Task ToggleAvailability_WhenNotFound_ReturnsNotFound()
    {
        _repoMock.Setup(r => r.ToggleAvailabilityAsync(99)).ReturnsAsync((Book?)null);

        var result = await _controller.ToggleAvailability(99);

        Assert.IsType<NotFoundResult>(result.Result);
    }
}
