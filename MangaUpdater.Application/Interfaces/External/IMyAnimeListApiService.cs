using MangaUpdater.Application.Models;

namespace MangaUpdater.Application.Interfaces.External;

public interface IMyAnimeListApiService
{
    Task<MyAnimeListApiResponse?> GetMangaFromMyAnimeListByIdAsync(int malMangaId);
}