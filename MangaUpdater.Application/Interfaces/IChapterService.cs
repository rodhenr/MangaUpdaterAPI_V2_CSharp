using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IChapterService
{
    Task AddChapter(Chapter chapter);
    Task BulkCreate(List<Chapter> chapters);
    Task<IEnumerable<Chapter>> GetChaptersByMangaId(int mangaId, int? max);
    Task<Chapter?> GetChapterById(int id);
    Task CreateOrUpdateChaptersByMangaSource(int mangaId, int sourceId, Dictionary<string, string> chapters);
}