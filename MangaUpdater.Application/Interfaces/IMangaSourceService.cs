using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaSourceService
{
    void Add(MangaSource mangaSource);
    Task<ICollection<MangaSource>> GetAllByMangaId(int mangaId);
    Task<MangaSource> GetByMangaIdAndSourceId(int mangaId, int sourceId);
    Task SaveChanges();
}