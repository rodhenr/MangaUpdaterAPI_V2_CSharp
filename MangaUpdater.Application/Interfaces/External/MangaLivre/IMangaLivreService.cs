namespace MangaUpdater.Application.Interfaces.External.MangaLivre;

public interface IMangaLivreService
{
    Task RegisterSourceAndChapters(int mangaId, int sourceId, string url);
    Task UpdateChapters(int mangaId, int sourceId, float lastChapterId, string url);
}