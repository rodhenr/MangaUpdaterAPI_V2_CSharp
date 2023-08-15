using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaSourceService
{
    Task AddMangaSource(MangaSource mangaSource);
    Task<IEnumerable<MangaSource>> GetAllByMangaId(int mangaId);
    Task<MangaSource?> GetByMangaIdAndSourceId(int mangaId, int sourceId);
}
