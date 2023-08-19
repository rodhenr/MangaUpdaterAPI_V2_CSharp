using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaSourceRepository
{
    Task CreateAsync(MangaSource mangaSource);
    Task<ICollection<MangaSource>> GetAllByMangaIdAsync(int mangaId);
    Task<ICollection<MangaSource>> GetAllBySourceIdAsync(int sourceId);
    Task<MangaSource?> GetByMangaIdAndSourceIdAsync(int mangaId, int sourceId);
}