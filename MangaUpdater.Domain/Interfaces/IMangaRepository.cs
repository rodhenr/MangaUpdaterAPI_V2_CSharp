using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaRepository
{
    Task<Manga> CreateMangaAsync(Manga manga);
    Task<Manga> GetMangaByIdAsync(int id);
    Task<IEnumerable<Manga>> GetMangasAsync();
}
