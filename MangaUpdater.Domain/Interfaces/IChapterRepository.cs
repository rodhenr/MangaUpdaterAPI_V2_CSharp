using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IChapterRepository : IBaseRepository<Chapter>
{
    void BulkCreate(IEnumerable<Chapter> chapters);
    Task<Chapter?> GetByIdAndMangaIdAsync(int mangaId, int chapterId);
    Task<Chapter?> GetSmallestChapterByMangaIdAsync(int mangaId, int sourceId);
    Task<Chapter?> GetLastChapterByMangaIdAndSourceIdAsync(int mangaId, int sourceId);
    Task<IEnumerable<Chapter>> GetThreeLastByMangaIdAndSourceListAsync(int mangaId, List<int> sourceList);
}