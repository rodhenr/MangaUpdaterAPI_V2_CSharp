using MangaUpdater.Application.Models;
using MangaUpdater.Application.Models.External.MyAnimeList;

namespace MangaUpdater.Application.Interfaces.External.MyAnimeList;

public interface IMyAnimeListApiService
{
    Task<MyAnimeListApiResponse?> GetMangaFromMyAnimeListByIdAsync(int malMangaId);
}