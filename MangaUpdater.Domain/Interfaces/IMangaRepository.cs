using MangaUpdater.Domain.Entities;

namespace CleanArchMvc.Domain.Interfaces;

public interface IMangaRepository
{
    Task<IEnumerable<Manga>> GetMangasAsync();
    Task<IEnumerable<Manga>> GetListByIdAsync(IEnumerable<int> ids);
    Task<Manga> GetByIdAsync(int id);
    Task<Manga> CreateAsync(Manga manga);
}
