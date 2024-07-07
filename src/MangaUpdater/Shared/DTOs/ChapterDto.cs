namespace MangaUpdater.Dto;

public record ChapterDto(int Id, int SourceId, string SourceName, DateTime Date, string Number, bool IsUserAllowedToRead = false, bool IsRead = false);