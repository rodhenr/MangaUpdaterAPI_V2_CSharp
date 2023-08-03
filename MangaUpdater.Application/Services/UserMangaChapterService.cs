using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class UserMangaChapterService : IUserMangaChapterService
{
    private readonly IUserMangaRepository _userMangaRepository;
    private readonly IChapterRepository _chapterRepository;

    public UserMangaChapterService(IUserMangaRepository userMangaRepository, IChapterRepository chapterRepository)
    {
        _userMangaRepository = userMangaRepository;
        _chapterRepository = chapterRepository;
    }

    public async Task AddUserManga(int mangaId, int userId, int sourceId)
    {
        var userManga = await _userMangaRepository.GetAllByMangaIdAndUserIdAsync(mangaId, userId);

        if (userManga.Any())
        {
            var chapter = await _chapterRepository.GetSmallestChapterByMangaIdAsync(mangaId, sourceId);

            if (chapter != null)
            {
                await _userMangaRepository.CreateAsync(new UserManga(userId, mangaId, sourceId, chapter.Id));
            }
        }

        return;
    }

    public async Task AddUserMangaBySourceIdList(int mangaId, int userId, IEnumerable<int> sourceIdList, IEnumerable<UserSourceDTO>? userSources)
    {
        foreach (var sourceId in sourceIdList)
        {
            if (userSources != null && userSources.Any(a => a.SourceId == sourceId && !a.IsFollowing))
            {
                var lastChapter = await _chapterRepository.GetSmallestChapterByMangaIdAsync(mangaId, sourceId);

                if (lastChapter != null)
                {
                    UserManga userManga = new(userId, mangaId, sourceId, lastChapter.Id);

                    await _userMangaRepository.CreateAsync(userManga);
                }
            }
        }

        return;
    }

    public async Task DeleteUserMangasByMangaId(int mangaId, int userId)
    {
        await _userMangaRepository.DeleteAllByMangaIdAndUserIdAsync(mangaId, userId);

        return;
    }

    public async Task DeleteUserManga(int mangaId, int userId, int sourceId)
    {
        var userManga = await _userMangaRepository.GetByMangaIdUserIdAndSourceIdAsync(mangaId, userId, sourceId);

        if (userManga != null)
        {
            await _userMangaRepository.DeleteAsync(userId, mangaId, sourceId);
        }

        return;
    }
}
