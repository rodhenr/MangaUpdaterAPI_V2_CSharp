using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaSourceRepository
{
    Task<IEnumerable<MangaSource>> GetByMangaIdAsync(int mangaId);
    Task<IEnumerable<MangaSource>> GetBySourceIdAsync(int sourceId);
}
