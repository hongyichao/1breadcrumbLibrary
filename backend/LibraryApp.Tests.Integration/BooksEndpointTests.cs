using System.Net;
using System.Net.Http.Json;
using LibraryApp.Api.Models;

namespace LibraryApp.Tests.Integration;

public class BooksEndpointTests : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _client;

    public BooksEndpointTests(WebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetBooks_ReturnsOkWithSeededBooks()
    {
        var response = await _client.GetAsync("/api/books");

        response.EnsureSuccessStatusCode();
        var books = await response.Content.ReadFromJsonAsync<List<BookDto>>();
        Assert.NotNull(books);
        Assert.NotEmpty(books);
    }

    [Fact]
    public async Task CreateBook_ThenGetById_ReturnsCreatedBook()
    {
        var request = new CreateBookRequest("Integration Book", "Test Author", "000-1", new DateOnly(2024, 1, 1), "Dave");

        var createResponse = await _client.PostAsJsonAsync("/api/books", request);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<BookDto>();
        Assert.NotNull(created);
        Assert.Equal("Integration Book", created.Title);
        Assert.True(created.IsAvailable);

        var getResponse = await _client.GetAsync($"/api/books/{created.Id}");
        getResponse.EnsureSuccessStatusCode();
        var fetched = await getResponse.Content.ReadFromJsonAsync<BookDto>();
        Assert.Equal(created.Id, fetched!.Id);
    }

    [Fact]
    public async Task DeleteBook_WhenExists_ReturnsNoContent()
    {
        var request = new CreateBookRequest("To Delete", "Author", "999", new DateOnly(2020, 1, 1), "Eve");
        var createResponse = await _client.PostAsJsonAsync("/api/books", request);
        var created = await createResponse.Content.ReadFromJsonAsync<BookDto>();

        var deleteResponse = await _client.DeleteAsync($"/api/books/{created!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/books/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task ToggleAvailability_FlipsIsAvailable()
    {
        var request = new CreateBookRequest("Toggle Me", "Author", "111", new DateOnly(2021, 1, 1), "Frank");
        var createResponse = await _client.PostAsJsonAsync("/api/books", request);
        var created = await createResponse.Content.ReadFromJsonAsync<BookDto>();
        Assert.True(created!.IsAvailable);

        var toggleResponse = await _client.PatchAsync($"/api/books/{created.Id}/availability", null);
        toggleResponse.EnsureSuccessStatusCode();
        var toggled = await toggleResponse.Content.ReadFromJsonAsync<BookDto>();

        Assert.False(toggled!.IsAvailable);
    }

    [Fact]
    public async Task GetBooks_WithSearchFilter_ReturnsMatchingBooks()
    {
        await _client.PostAsJsonAsync("/api/books", new CreateBookRequest("Unique Title XYZ", "Author", "222", new DateOnly(2022, 1, 1), "Grace"));

        var response = await _client.GetAsync($"/api/books?search={Uri.EscapeDataString("Unique Title XYZ")}");
        response.EnsureSuccessStatusCode();
        var books = await response.Content.ReadFromJsonAsync<List<BookDto>>();

        Assert.NotNull(books);
        Assert.All(books, b => Assert.Contains("Unique Title XYZ", b.Title));
    }
}
