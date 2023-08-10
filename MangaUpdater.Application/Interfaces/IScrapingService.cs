using MangaUpdater.Application.Models;

namespace MangaUpdater.Application.Interfaces;

public interface IScrapingService
{
    MangaRegister GetAsuraAsync();
    MangaRegister GetMangaLivreAsync();
}
