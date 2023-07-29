using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;
public interface IUserMangaRepository
{
    Task<IEnumerable<UserManga>> GetByUserIdAsync(int userId);
    Task<IEnumerable<UserManga>> GetByMangaIdAsync(int mangaId);
    Task<IEnumerable<UserManga>> GetByMangaIdAndUserIdAsync(int mangaId, int userId);
}
