using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;
public interface IUserMangaRepository
{
    Task CreateAsync(UserManga userManga);
    Task<IEnumerable<UserManga>> GetAllByMangaIdAsync(int mangaId);
    Task<IEnumerable<UserManga>> GetAllByUserIdAsync(int userId);
    Task<IEnumerable<UserManga>> GetAllByMangaIdAndUserIdAsync(int mangaId, int userId);
    Task<UserManga?> GetByMangaIdUserIdAndSourceIdAsync(int mangaId, int userId, int sourceId);
    Task UpdateAsync(int userId, int mangaId, int sourceId, int chapterId);
    Task DeleteAsync(int userId, int mangaId, int sourceId);
    Task DeleteAllByMangaIdAndUserIdAsync(int mangaId, int UserId);
}