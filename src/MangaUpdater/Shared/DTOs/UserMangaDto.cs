namespace MangaUpdater.Dto;

public record UserMangaDto(int MangaId, int SourceId, int? ChapterId, string? Number);