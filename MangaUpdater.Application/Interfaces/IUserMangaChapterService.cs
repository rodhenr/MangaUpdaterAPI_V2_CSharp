using MangaUpdater.Application.DTOs;

namespace MangaUpdater.Application.Interfaces;

public interface IUserMangaChapterService
{
    Task AddUserManga(int mangaId, int userId, int sourceId);
    Task AddUserMangaBySourceIdList(int mangaId, int userId, IEnumerable<int> sourceIdList, IEnumerable<UserSourceDTO>? userSources);
    Task DeleteUserMangasByMangaId(int mangaId, int userId);
    Task DeleteUserManga(int mangaId, int userId, int sourceId);
}
