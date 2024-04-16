using MangaUpdater.Core.Dtos;
using MangaUpdater.Data.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IUserMangaService
{
    Task<IEnumerable<MangaUserDto>> GetMangasByUserId(string userId);
    Task<UserManga?> GetByUserIdAndMangaId(string userId, int mangaId);
    Task<UsersFollowingMangaDto> GetTotalNumberOfUsersFollowingById(int mangaId);
    Task Update(UserManga userManga);
}