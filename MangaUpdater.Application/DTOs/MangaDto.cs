namespace MangaUpdater.Application.DTOs;

public record MangaDto(string CoverUrl, string Name, string AlternativeName, string Author, string Synopsis,
    string Type,
    int MyAnimeListId, bool IsUserFollowing, IEnumerable<string>? Genres, IEnumerable<SourceDto>? Sources,
    IEnumerable<ChapterDto>? Chapters);