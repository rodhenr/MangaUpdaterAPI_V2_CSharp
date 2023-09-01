using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IUserMangaRepository: IBaseRepository<UserManga>
{
    Task<IEnumerable<UserManga>> GetAllByMangaIdAsync(int mangaId);
    Task<IEnumerable<UserManga>> GetAllByUserIdAsync(string userId);
    Task<IEnumerable<UserManga>> GetAllByMangaIdAndUserIdAsync(int mangaId, string userId);
    Task<UserManga?> GetByMangaIdUserIdAndSourceIdAsync(int mangaId, string userId, int sourceId);
    Task DeleteAllByMangaIdAndUserIdAsync(int mangaId, string userId);
}