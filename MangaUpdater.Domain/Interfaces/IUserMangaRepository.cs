using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IUserMangaRepository : IBaseRepository<UserManga>
{
    Task<IEnumerable<UserManga>> GetAllByUserIdWithPageLimitAsync(string userId, int page, int limit);
    Task<IEnumerable<UserManga>> GetAllByUserIdAsync(string userId);
    Task<UserManga?> GetByMangaIdAndUserIdAsync(int mangaId, string userId);
    Task DeleteAsync(int mangaId, string userId);
    Task SaveChangesAsync(UserManga userManga);
    Task<IEnumerable<UserManga>> GetMangasToUpdateChaptersAsync(string userId);
    Task<int> GetTotalNumberOfUsersFollowingByIdAsync(int mangaId);
}