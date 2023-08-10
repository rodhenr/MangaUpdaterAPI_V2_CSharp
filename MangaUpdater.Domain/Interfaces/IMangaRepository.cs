using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaRepository
{
    Task CreateAsync(Manga manga);
    Task<IEnumerable<Manga>> GetAsync();
    Task<IEnumerable<Manga>> GetWithFiltersAsync(string? orderBy, List<int>? sourceIdList, List<int>? genreIdList);
    Task<IEnumerable<Manga>> GetAllByUserId(int userId);
    Task<IEnumerable<Manga>> GetAllByUserLoggedIdWithLastThreeChapters(int userId);
    Task<Manga?> GetByMalIdAsync(int malId);
    Task<Manga?> GetByIdOrderedDescAsync(int id);
    Task<Manga?> GetByIdAndUserIdOrderedDescAsync(int id, int userId);
}
