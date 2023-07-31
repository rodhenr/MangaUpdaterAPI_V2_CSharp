using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;
public interface IUserMangaRepository
{
    Task CreateAsync(UserManga userManga);
    Task<IEnumerable<UserManga>> GetByUserIdAsync(int userId);
    Task<IEnumerable<UserManga>> GetByMangaIdAsync(int mangaId);
    Task<IEnumerable<UserManga>> GetByMangaIdAndUserIdAsync(int mangaId, int userId);
    Task UpdateAsync(int userId, int mangaId, int sourceId, int chapterId);
}
