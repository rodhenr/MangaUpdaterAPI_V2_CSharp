using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IScrapingService
{
    MangaRegister GetAsuraAsync();
    MangaRegister GetMangaLivreAsync();
}
