using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class UserSourceService : IUserSourceService
{
    private readonly IUserMangaRepository _userMangaRepository;
    private readonly IMangaSourceRepository _mangaSourceRepository;

    public UserSourceService(IUserMangaRepository userMangaRepository, IMangaSourceRepository mangaSourceRepository)
    {
        _userMangaRepository = userMangaRepository;
        _mangaSourceRepository = mangaSourceRepository;
    }

    public async Task<IEnumerable<UserSourceDto>?> GetUserSourcesByMangaId(int mangaId, string userId)
    {
        var mangaSources = await _mangaSourceRepository.GetAllByMangaIdAsync(mangaId);
        var userMangas = await _userMangaRepository.GetAllByMangaIdAndUserIdAsync(mangaId, userId);

        return mangaSources
            .Select(ms =>
                new UserSourceDto(ms.SourceId, ms.Source!.Name, userMangas.Any(b => b.SourceId == ms.SourceId)))
            .ToList();
    }
}