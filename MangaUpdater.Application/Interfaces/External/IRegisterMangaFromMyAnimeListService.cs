using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IRegisterMangaFromMyAnimeListService
{
    Task<Manga?> RegisterMangaFromMyAnimeListById(int malMangaId);
}