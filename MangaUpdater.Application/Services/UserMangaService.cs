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

    public async Task<IEnumerable<UserManga>> GetMangasByUserId(int userId)
    {
        return await _userMangaRepository.GetByUserIdAsync(userId);
    }
}
