namespace MangaUpdater.Application.Interfaces;

public interface IUserMangaChapterService
{
    Task AddUserMangaBySourceIdList(int mangaId, int userId, IEnumerable<int> sourceIdList);
}
