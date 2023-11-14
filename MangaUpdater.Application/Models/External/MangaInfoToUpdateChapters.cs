namespace MangaUpdater.Application.Models.External;

public record MangaInfoToUpdateChapters(int MangaId, int SourceId, string MangaUrl, string SourceBaseUrl,
    string SourceName, string? LastSavedChapter);