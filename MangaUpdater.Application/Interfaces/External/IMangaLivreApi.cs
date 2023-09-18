using MangaUpdater.Application.Models;

namespace MangaUpdater.Application.Interfaces.External;

public interface IMangaLivreApi
{
    Task<List<MangaLivreChapters>> GetChaptersAsync(int mlSerieId, float lastChapterId = 0);
}