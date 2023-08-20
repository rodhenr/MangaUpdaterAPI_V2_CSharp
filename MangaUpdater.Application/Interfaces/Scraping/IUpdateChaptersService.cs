using MangaUpdater.Application.Models;

namespace MangaUpdater.Application.Interfaces.Scraping;

public interface IUpdateChaptersService
{
    //Task<MangaRegister> GetAsuraAsync();
    Dictionary<string, string> UpdateChaptersFromMangaLivreSource(string sourceUrl, string linkUrl);
}
