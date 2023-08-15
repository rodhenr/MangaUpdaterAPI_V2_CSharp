using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IRegisterMangaService
{
    Task<Manga?> RegisterMangaFromMyAnimeListById(int malMangaId);
}
