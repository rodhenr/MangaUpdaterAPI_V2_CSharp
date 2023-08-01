using MangaUpdater.Application.DTOs;

namespace MangaUpdater.Application.Interfaces;

public interface IUserMangaChapterService
{
    Task AddUserMangaBySourceIdList(int mangaId, int userId, IEnumerable<int> sourceIdList, IEnumerable<UserSourceDTO> userSources);
    Task AddUserSource(int mangaId, int userId, int sourceId);
    Task DeleteUserSource(int mangaId, int userId, int sourceId);
    Task DeleteAllUserSources(int mangaId, int userId);
}
