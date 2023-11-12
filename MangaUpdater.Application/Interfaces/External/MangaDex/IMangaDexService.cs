namespace MangaUpdater.Application.Interfaces.External.MangaDex;

public interface IMangaDexService
{
    Task UpdateChapters(int mangaId, int sourceId,string mangaDexId);
}