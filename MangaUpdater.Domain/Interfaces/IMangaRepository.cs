using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaRepository : IBaseRepository<Manga>
{
    IQueryable<Manga> GetWithFiltersAsync(string? orderBy, List<int>? sourceIdList, List<int>? genreIdList,
        string? input);

    Task<Manga?> GetByMalIdAsync(int malId);
    Task<Manga?> GetByIdOrderedDescAsync(int id);
    Task<Manga?> GetByIdAndUserIdOrderedDescAsync(int id, string userId);
}