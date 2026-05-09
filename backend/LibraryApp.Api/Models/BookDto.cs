namespace LibraryApp.Api.Models;

public record BookDto(
    int Id,
    string Title,
    string Author,
    string Isbn,
    DateOnly PublishedDate,
    string Owner,
    bool IsAvailable
);
