using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaRepository
{
    Task CreateAsync(Manga manga);
    Task<IEnumerable<Manga>> GetMangasAsync();
    Task<Manga?> GetByIdAsync(int id);
    Task<Manga?> GetByIdAndUserIdAsync(int id, int userId);
}
