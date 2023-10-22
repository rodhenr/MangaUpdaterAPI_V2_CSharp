using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class UserSourceService : IUserSourceService
{
    private readonly IUserChapterRepository _userChapterRepository;
    private readonly IMangaSourceRepository _mangaSourceRepository;

    public UserSourceService(IMangaSourceRepository mangaSourceRepository, IUserChapterRepository userChapterRepository)
    {
        _mangaSourceRepository = mangaSourceRepository;
        _userChapterRepository = userChapterRepository;
    }

    public async Task<IEnumerable<UserSourceDto>> GetUserSourcesByMangaId(int mangaId, int? userMangaId)
    {
        var mangaSources = await _mangaSourceRepository.GetAllByMangaIdAsync(mangaId);
        var userChapters = await _userChapterRepository.GetByUserMangaIdAsync(userMangaId ?? 0);

        return mangaSources
            .Select(ms =>
                new UserSourceDto(ms.SourceId, ms.Source!.Name, userChapters.Any(b => b.SourceId == ms.SourceId)))
            .ToList();
    }
}