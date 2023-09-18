using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IChapterService
{
    void Add(Chapter chapter);
    void BulkCreate(IEnumerable<Chapter> chapters);
    Task<Chapter> GetById(int id);
    Task<IEnumerable<Chapter>> GetByMangaId(int mangaId, int? max);
    Task CreateOrUpdateByMangaSource(int mangaId, int sourceId, Dictionary<string, string> chapters);
    Task<Chapter?> GetLastByMangaIdAndSourceId(int mangaId, int sourceId);
    Task SaveChanges();
}