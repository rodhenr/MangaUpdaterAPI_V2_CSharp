namespace MangaUpdater.Application.DTOs;

public record MangaUserLoggedDto(int Id, string CoverUrl, string Name, IEnumerable<ChapterDto>? Chapters);