using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IChapterService
{
    void BulkCreate(IEnumerable<Chapter> chapters);
    Task<Chapter> GetByIdAndMangaId(int mangaId, int chapterId);
    Task<Chapter?> GetLastByMangaIdAndSourceId(int mangaId, int sourceId);
}