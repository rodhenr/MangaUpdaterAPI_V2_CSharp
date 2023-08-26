using MangaUpdater.Application.DTOs;

namespace MangaUpdater.Application.Interfaces;

public interface IUserMangaChapterService
{
    Task AddUserManga(int mangaId, string userId, int sourceId);
    Task AddUserMangaBySourceIdList(int mangaId, string userId, IEnumerable<int> sourceIdList, IEnumerable<UserSourceDTO>? userSources);
    Task<IEnumerable<MangaUserLoggedDTO>> GetUserMangasWithThreeLastChapterByUserId(string userId);
    Task DeleteUserMangasByMangaId(int mangaId, string userId);
    Task DeleteUserManga(int mangaId, string userId, int sourceId);
}
