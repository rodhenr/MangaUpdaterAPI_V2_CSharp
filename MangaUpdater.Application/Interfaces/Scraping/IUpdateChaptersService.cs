using MangaUpdater.Application.Models;

namespace MangaUpdater.Application.Interfaces.Scraping;

public interface IUpdateChaptersService
{
    //Task<MangaRegister> GetAsuraAsync();
    Task UpdateChaptersFromMangaLivreSource(string sourceUrl, string linkUrl);
}
