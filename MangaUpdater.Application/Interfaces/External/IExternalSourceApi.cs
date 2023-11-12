using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces.External;

public interface IExternalSourceApi
{
    Task<List<Chapter>> GetChaptersAsync(int mangaId, int sourceId, string mangaUrl, string sourceUrl,
        string? initialChapter);
}