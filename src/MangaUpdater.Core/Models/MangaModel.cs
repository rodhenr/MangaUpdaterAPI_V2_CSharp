namespace MangaUpdater.Core.Models;

public record SourceDto(int Id, string Name, string BaseUrl);
public record ChapterDto
{
    public required int ChapterId { get; init; }
    public required int SourceId { get; init; }
    public required string SourceName { get; init; }
    public required DateTime Date { get; init; }
    public required string Number { get; init; }
    public required bool IsUserAllowedToRead { get; init; }
    public required bool Read { get; init; }
};

public record MangaModel
{
    public required int MangaId { get; init; }
    public required string CoverUrl { get; init; }
    public required string Name { get; init; }
    public required string AlternativeName { get; init; }
    public required string Author { get; init; }
    public required string Synopsis { get; init; }
    public required string Type { get; init; }
    public required int MyAnimeListId { get; init; }
    public required bool IsUserFollowing { get; init; }
    public required IEnumerable<SourceDto> Sources { get; init; }
    public required IEnumerable<string> Genres { get; init; }
    public required IEnumerable<ChapterDto> Chapters { get; init; }
};