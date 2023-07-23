using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaService
{
    Task<IEnumerable<Manga>> GetMangas();
    Task<IEnumerable<Manga>> GetUserMangas(IEnumerable<int> ids);
    Task<Manga> GetMangaById(int id);
    Task AddManga(Manga manga);
}
