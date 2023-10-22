using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IUserChapterRepository : IBaseRepository<UserChapter>
{
    Task<IEnumerable<UserChapter>> GetByUserMangaIdAsync(int userMangaId);
    Task<UserChapter?> GetByUserMangaIdAndSourceIdAsync(int userMangaId, int sourceId);
    Task DeleteAsync(int userMangaId);
    Task DeleteAsync(int userMangaId, int sourceId);
}