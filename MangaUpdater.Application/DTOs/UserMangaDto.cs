namespace MangaUpdater.Application.DTOs;

public record UserMangaDto(string UserId, int MangaId, int SourceId, int CurrentChapterId);