using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class UserChapterService : IUserChapterService
{
    private readonly IUserChapterRepository _userChapterRepository;

    public UserChapterService(IUserChapterRepository userChapterRepository)
    {
        _userChapterRepository = userChapterRepository;
    }

    public async Task<IEnumerable<UserChapter>> GetByUserMangaId(int userMangaId)
    {
        return await _userChapterRepository.GetByUserMangaIdAsync(userMangaId);
    }

    public async Task<UserChapter?> GetByUserMangaIdAndSourceIdAsync(int userMangaId, int sourceId)
    {
        return await _userChapterRepository.GetByUserMangaIdAndSourceIdAsync(userMangaId, sourceId);
    }
}