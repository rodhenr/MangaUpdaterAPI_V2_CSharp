namespace MangaUpdater.Core.Dtos;

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