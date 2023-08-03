using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaSourceRepository
{
    Task<IEnumerable<MangaSource>> GetAllByMangaIdAsync(int mangaId);
    Task<IEnumerable<MangaSource>> GetAllBySourceIdAsync(int sourceId);
}