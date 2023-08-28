namespace MangaUpdater.Application.DTOs;

public record ChapterDto(int ChapterId, int SourceId, string SourceName, DateTime Date, float Number, bool Read);