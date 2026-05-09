using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Api.Models;

public record CreateBookRequest(
    [Required][MaxLength(255)] string Title,
    [Required][MaxLength(255)] string Author,
    [MaxLength(20)] string Isbn,
    DateOnly PublishedDate,
    [Required][MaxLength(255)] string Owner
);
