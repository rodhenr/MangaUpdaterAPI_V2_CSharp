using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IChapterRepository: IRepository<Chapter>
{
    Task BulkCreateAsync(IEnumerable<Chapter> chapters);
    Task<IEnumerable<Chapter>> GetByMangaIdAsync(int mangaId, int max);
    Task<Chapter?> GetSmallestChapterByMangaIdAsync(int mangaId, int sourceId);
    Task<ICollection<Chapter>> GetThreeLastByMangaIdAndSourceListAsync(int mangaId, List<int> sourceList);
    Task<IEnumerable<float>> GetChaptersNumberByMangaIdAndSourceIdAsync(int mangaId, int sourceId);
}