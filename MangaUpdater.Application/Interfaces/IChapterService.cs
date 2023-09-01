using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IChapterService
{
    Task Add(Chapter chapter);
    Task BulkCreate(IEnumerable<Chapter> chapters);
    Task<Chapter?> GetById(int id);
    Task<IEnumerable<Chapter>> GetByMangaId(int mangaId, int? max);
    Task CreateOrUpdateByMangaSource(int mangaId, int sourceId, Dictionary<string, string> chapters);
}