using MangaUpdater.Application.Models.External.MangaLivre;

namespace MangaUpdater.Application.Interfaces.External.MangaLivre;

public interface IMangaLivreApi
{
    Task<List<MangaLivreChapters>> GetChaptersAsync(int mlSerieId, float lastChapterId = 0);
}