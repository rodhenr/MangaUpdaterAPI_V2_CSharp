using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IUserMangaService
{
    Task<IEnumerable<UserManga>> GetMangasByUserId(int userId);
    Task<IEnumerable<UserManga>> GetMangasByMangaId(int mangaId);
    Task<IEnumerable<UserManga>> GetByMangaIdAndUserId(int mangaId, int userId);
    Task UpdateUserMangaAsync(int userId, int mangaId, int sourceId, int chapterId);
}
