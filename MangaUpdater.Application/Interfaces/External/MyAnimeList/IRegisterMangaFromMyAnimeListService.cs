using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces.External.MyAnimeList;

public interface IRegisterMangaFromMyAnimeListService
{
    Task<Manga?> RegisterMangaFromMyAnimeListById(int malMangaId);
}