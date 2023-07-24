using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaRepository
{
    Task<Manga> CreateAsync(Manga manga);
    Task<IEnumerable<Manga>> GetAsync();
    Task<Manga> GetByIdAsync(int id);
}
