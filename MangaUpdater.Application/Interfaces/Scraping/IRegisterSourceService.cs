using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces.Scraping;

public interface IRegisterSourceService
{
    Task RegisterFromMangaLivreSource(int mangaId, int sourceId, string sourceUrl, string linkUrl,
        IEnumerable<MangaTitle> mangaTitles);

    Task RegisterFromAsuraScansSource(int mangaId, int sourceId, string sourceUrl, string linkUrl,
        IEnumerable<MangaTitle> mangaTitles);
}