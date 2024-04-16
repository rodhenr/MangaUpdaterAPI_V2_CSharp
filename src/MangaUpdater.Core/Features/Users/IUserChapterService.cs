using MangaUpdater.Data.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IUserChapterService
{
    Task<IEnumerable<UserChapter>> GetByUserMangaId(int userMangaId);
    Task<UserChapter?> GetByUserMangaIdAndSourceIdAsync(int userMangaId, int sourceId);
}