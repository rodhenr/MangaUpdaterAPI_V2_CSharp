namespace MangaUpdater.Application.DTOs;

public record ChapterDto
{
    public required int ChapterId { get; init; }
    public required int SourceId { get; init; }
    public required string SourceName { get; init; }
    public required DateTime Date { get; init; }
    public required float Number { get; init; }
    public required bool Read { get; init; }
};