using MangaUpdater.Application.DTOs;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IUserMangaService
{
    Task<IEnumerable<MangaUserDto>> GetMangasByUserId(string userId);
    Task<UserManga?> GetByUserIdAndMangaId(string userId, int mangaId);
    Task Update(UserManga userManga);
}