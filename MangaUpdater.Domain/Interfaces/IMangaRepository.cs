using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaRepository
{
    Task CreateAsync(Manga manga);
    Task<IEnumerable<Manga>> GetAsync();
    Task<IEnumerable<Manga>> GetWithFiltersAsync(string? orderBy, List<int>? sourceIdList, List<int>? genreIdList);
    Task<Manga?> GetByMalIdAsync(int malId);
    Task<Manga?> GetByIdOrderedDescAsync(int id);
    Task<Manga?> GetByIdAndUserIdOrderedDescAsync(int id, string userId);
}
