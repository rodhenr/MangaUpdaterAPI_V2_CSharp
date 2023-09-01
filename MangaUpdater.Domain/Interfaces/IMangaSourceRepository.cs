using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaSourceRepository: IBaseRepository<MangaSource>
{
    Task<ICollection<MangaSource>> GetAllByMangaIdAsync(int mangaId);
    Task<MangaSource?> GetByMangaIdAndSourceIdAsync(int mangaId, int sourceId);
}