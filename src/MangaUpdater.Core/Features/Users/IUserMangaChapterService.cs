using MangaUpdater.Core.Dtos;

namespace MangaUpdater.Application.Interfaces;

public interface IUserMangaChapterService
{
    Task AddUserMangaBySourceIdList(int mangaId, string userId, IEnumerable<int> sourceIdList);
    Task<IEnumerable<MangaUserLoggedDto>> GetUserMangasWithThreeLastChapterByUserId(string userId, int page, int limit);
    Task DeleteUserMangasByMangaId(int mangaId, string userId);
    Task DeleteUserMangaByMangaIdAndSourceId(int mangaId, int sourceId, string userId);
    Task UpdateOrCreateUserChapter(string userId, int mangaId, int sourceId, int chapterId);
}