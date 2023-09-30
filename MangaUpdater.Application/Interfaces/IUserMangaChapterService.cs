using MangaUpdater.Application.DTOs;

namespace MangaUpdater.Application.Interfaces;

public interface IUserMangaChapterService
{
    Task AddUserMangaBySourceIdList(int mangaId, string userId, IEnumerable<int> sourceIdList);
    Task<IEnumerable<MangaUserLoggedDto>> GetUserMangasWithThreeLastChapterByUserId(string userId);
    Task DeleteUserMangasByMangaId(int mangaId, string userId);
    Task DeleteUserMangaByMangaIdAndSourceId(int mangaId, int sourceId, string userId);
}