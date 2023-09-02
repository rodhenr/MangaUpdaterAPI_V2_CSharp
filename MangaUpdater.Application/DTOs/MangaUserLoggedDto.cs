namespace MangaUpdater.Application.DTOs;

public record MangaUserLoggedDto
{
    public required int Id { get; init; }
    public required string CoverUrl { get; init; }
    public required string Name { get; init; }
    public IEnumerable<ChapterDto>? Chapters { get; init; }
};