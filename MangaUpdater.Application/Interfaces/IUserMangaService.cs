using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Models.External;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IUserMangaService
{
    Task<IEnumerable<MangaUserDto>> GetMangasByUserId(string userId);
    Task<UserManga?> GetByUserIdAndMangaId(string userId, int mangaId);
    Task Update(UserManga userManga);
    Task<List<MangaInfoToUpdateChapters>> GetMangasToUpdateChapters(string userId);
}