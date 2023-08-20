using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IChapterRepository
{
    Task CreateAsync(Chapter chapter);
    Task BulkCreateAsync(List<Chapter> chapters);
    Task<IEnumerable<Chapter>> GetAllByMangaIdAsync(int mangaId, int max);
    Task<Chapter?> GetByIdAsync(int id);
    Task<Chapter?> GetSmallestChapterByMangaIdAsync(int mangaId, int sourceId);
    Task<ICollection<Chapter>> GetThreeLastByMangaIdAndSourceListAsync(int mangaId, List<int> sourceList);
    Task<IEnumerable<float>> GetChaptersNumberByMangaIdAndSourceIdAsync(int mangaId, int sourceId);
}
