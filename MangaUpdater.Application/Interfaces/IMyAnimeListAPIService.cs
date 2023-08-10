using MangaUpdater.Application.Models;

namespace MangaUpdater.Application.Interfaces;

public interface IMyAnimeListAPIService
{
    Task<MyAnimeListAPIResponse?> GetMangaByIdAsync(int malMangaId);
}
