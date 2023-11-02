using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaRepository : IBaseRepository<Manga>
{
    Task<IEnumerable<Manga>> GetWithFiltersAsync(int page, int pageSize, string? orderBy, List<int>? sourceIdList,
        List<int>? genreIdList);

    Task<Manga?> GetByMalIdAsync(int malId);
    Task<Manga?> GetByIdOrderedDescAsync(int id);
    Task<Manga?> GetByIdAndUserIdOrderedDescAsync(int id, string userId);
    Task<int> CheckNumberOfPagesAsync(int pageSize);
}