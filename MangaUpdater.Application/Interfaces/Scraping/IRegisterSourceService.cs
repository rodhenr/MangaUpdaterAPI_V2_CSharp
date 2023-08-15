namespace MangaUpdater.Application.Interfaces.Scraping;

public interface IRegisterSourceService
{
    //Task<MangaRegister> GetAsuraAsync();
    Dictionary<string, string> RegisterFromMangaLivreSource(string sourceUrl, string linkUrl, string mangaName);
}
