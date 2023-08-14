using MangaUpdater.Application.DTOs;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IUserMangaService
{
    Task<IEnumerable<UserManga>> GetUserMangasByMangaId(int mangaId);
    Task<IEnumerable<UserManga>> GetUserMangasByMangaIdAndUserId(int mangaId, int userId);
    Task<IEnumerable<MangaUserDTO>> GetMangasByUserId(int userId);
    Task UpdateUserMangaAsync(int userId, int mangaId, int sourceId, int chapterId);
}
