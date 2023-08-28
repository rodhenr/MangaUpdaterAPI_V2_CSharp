using MangaUpdater.Application.Models;

namespace MangaUpdater.Application.Interfaces;

public interface IMyAnimeListApiService
{
    Task<MyAnimeListApiResponse?> GetMangaByIdAsync(int malMangaId);
}