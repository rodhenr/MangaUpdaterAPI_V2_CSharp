using MangaUpdater.Application.DTOs;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IUserMangaService
{
    Task<IEnumerable<UserManga>> GetByMangaId(int mangaId);
    Task<IEnumerable<UserManga>> GetByMangaIdAndUserId(int mangaId, string userId);
    Task<IEnumerable<MangaUserDto>> GetMangasByUserId(string userId);
    Task<UserManga> GetByMangaIdUserIdAndSourceId(int mangaId, string userId, int sourceId);
    Task Update(UserManga userManga);
}