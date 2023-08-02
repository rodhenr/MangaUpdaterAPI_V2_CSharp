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

    public async Task AddUserMangaBySourceIdList(int mangaId, int userId, IEnumerable<int> sourceIdList, IEnumerable<UserSourceDTO> userSources)
    {
        foreach (var sourceId in sourceIdList)
        {
            if (userSources.Any(a => a.SourceId == sourceId && !a.IsFollowing))
            {
                var lastChapter = await _chapterRepository.GetSmallestChapter(mangaId, sourceId);

                if (lastChapter != null)
                {
                    UserManga userManga = new(userId, mangaId, sourceId, lastChapter.Id);

                    await _userMangaRepository.CreateAsync(userManga);
                }
            }
        }

        return;
    }

    public async Task DeleteUserSource(int mangaId, int userId, int sourceId)
    {
        var userManga = await _userMangaRepository.GetByMangaIdUserIdAndSourceIdAsync(mangaId, userId, sourceId);

        if (userManga != null)
        {
            await _userMangaRepository.DeleteAsync(userId, mangaId, sourceId);
        }

        return;
    }

    public async Task AddUserSource(int mangaId, int userId, int sourceId)
    {
        var userManga = await _userMangaRepository.GetByMangaIdAndUserIdAsync(mangaId, userId);

        if (userManga.Any())
        {
            var chapter = await _chapterRepository.GetSmallestChapter(mangaId, sourceId);

            if (chapter != null)
            {
                await _userMangaRepository.CreateAsync(new UserManga(userId, mangaId, sourceId, chapter.Id));
            }
        }

        return;
    }

    public async Task DeleteAllUserSources(int mangaId, int userId)
    {
        await _userMangaRepository.DeleteByMangaIdAndUserIdAsync(mangaId, userId);

        return;
    }
}
