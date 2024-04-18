namespace MangaUpdater.Core.Models;

public record MangaInfoToUpdateModel(int MangaId, int SourceId, string MangaUrl, string SourceBaseUrl,
    string SourceName, string? LastSavedChapter);