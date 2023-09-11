namespace MangaUpdater.Application.Interfaces.Scraping;

public interface IRegisterSourceService
{
    Task RegisterFromMangaLivreSource(int mangaId, int sourceId, string sourceUrl, string linkUrl, string mangaName);
    Task RegisterFromAsuraScansSource(int mangaId, int sourceId, string sourceUrl, string linkUrl, string mangaName);
}