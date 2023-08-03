using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class UserMangaService : IUserMangaService
{
    private readonly IUserMangaRepository _userMangaRepository;

    public UserMangaService(IUserMangaRepository userMangaRepository)
    {
        _userMangaRepository = userMangaRepository;
    }

    public async Task<IEnumerable<UserManga>> GetByMangaIdAndUserId(int mangaId, int userId)
    {
        return await _userMangaRepository.GetAllByMangaIdAndUserIdAsync(mangaId, userId);
    }

    public async Task<IEnumerable<UserManga>> GetMangasByMangaId(int mangaId)
    {
        return await _userMangaRepository.GetAllByMangaIdAsync(mangaId);
    }

    public async Task<IEnumerable<UserManga>> GetMangasByUserId(int userId)
    {
        return await _userMangaRepository.GetAllByUserIdAsync(userId);
    }

    public async Task UpdateUserMangaAsync(int userId, int mangaId, int sourceId, int chapterId)
    {
        await _userMangaRepository.UpdateAsync(userId,mangaId,sourceId,chapterId);

        return;
    }
}
