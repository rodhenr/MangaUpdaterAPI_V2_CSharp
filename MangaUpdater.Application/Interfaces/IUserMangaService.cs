using MangaUpdater.Application.DTOs;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IUserMangaService
{
    Task<IEnumerable<UserManga>> GetUserMangasByMangaId(int mangaId);
    Task<IEnumerable<UserManga>> GetUserMangasByMangaIdAndUserId(int mangaId, string userId);
    Task<IEnumerable<MangaUserDTO>> GetMangasByUserId(string userId);
    Task UpdateUserMangaAsync(string userId, int mangaId, int sourceId, int chapterId);
}
