namespace MangaUpdater.Application.Interfaces.External;

public interface IMangaLivreService
{
    Task RegisterSourceAndChapters(int mangaId, int sourceId, string url);
    Task UpdateChapters(int mangaId, int sourceId, float lastChapterId, string url);
}