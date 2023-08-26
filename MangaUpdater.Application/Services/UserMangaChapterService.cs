using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Models;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class UserMangaChapterService : IUserMangaChapterService
{
    private readonly IUserMangaRepository _userMangaRepository;
    private readonly IChapterRepository _chapterRepository;
    private readonly IMapper _mapper;

    public UserMangaChapterService(IUserMangaRepository userMangaRepository, IChapterRepository chapterRepository, IMapper mapper)
    {
        _userMangaRepository = userMangaRepository;
        _chapterRepository = chapterRepository;
        _mapper = mapper;
    }

    public async Task AddUserManga(int mangaId, string userId, int sourceId)
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

    public async Task AddUserMangaBySourceIdList(int mangaId, string userId, IEnumerable<int> sourceIdList, IEnumerable<UserSourceDTO>? userSources)
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

    public async Task<IEnumerable<MangaUserLoggedDTO>> GetUserMangasWithThreeLastChapterByUserId(string userId)
    {
        var userMangas = await _userMangaRepository.GetAllByUserIdAsync(userId);

        var userMangasByMangaId = userMangas
             .GroupBy(a => a.MangaId)
             .Select(group => new UserMangaGroupByManga(group.Select(c => c.Manga).FirstOrDefault()!, group.Select(a => new SourceWithLastChapterRead(a.SourceId, a.Source!.Name, a.CurrentChapterId)).ToList()))
             .ToList();

        foreach (UserMangaGroupByManga userManga in userMangasByMangaId)
        {
            var chapters = await _chapterRepository.GetThreeLastByMangaIdAndSourceListAsync(userManga.Manga.Id, userManga.SourcesWithLastChapterRead.Select(a => a.SourceId).ToList());

            userManga.Manga.Chapters = chapters;
        }

        return _mapper.Map<IEnumerable<MangaUserLoggedDTO>>(userMangasByMangaId);
    }

    public async Task DeleteUserMangasByMangaId(int mangaId, string userId)
    {
        await _userMangaRepository.DeleteAllByMangaIdAndUserIdAsync(mangaId, userId);

        return;
    }

    public async Task DeleteUserManga(int mangaId, string userId, int sourceId)
    {
        var userManga = await _userMangaRepository.GetByMangaIdUserIdAndSourceIdAsync(mangaId, userId, sourceId);

        if (userManga != null)
        {
            await _userMangaRepository.DeleteAsync(userId, mangaId, sourceId);
        }

        return;
    }
}
