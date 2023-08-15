using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaSourceRepository
{
    Task CreateAsync(MangaSource mangaSource);
    Task<IEnumerable<MangaSource>> GetAllByMangaIdAsync(int mangaId);
    Task<IEnumerable<MangaSource>> GetAllBySourceIdAsync(int sourceId);
    Task<MangaSource?> GetByMangaIdAndSourceIdAsync(int mangaId, int sourceId);
}