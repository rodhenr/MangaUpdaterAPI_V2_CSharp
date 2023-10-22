using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IUserMangaRepository : IBaseRepository<UserManga>
{
    Task<IEnumerable<UserManga>> GetAllByUserIdAsync(string userId);
    Task<UserManga?> GetByMangaIdAndUserIdAsync(int mangaId, string userId);
    Task DeleteAsync(int mangaId, string userId);
    Task SaveChangesAsync(UserManga userManga);
}