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

    public UserMangaChapterService(IUserMangaRepository userMangaRepository, IChapterRepository chapterRepository,
        IMapper mapper)
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
                await _userMangaRepository.CreateAsync(new UserManga(userId, mangaId, sourceId, chapter.Id));
        }
    }

    public async Task AddUserMangaBySourceIdList(int mangaId, string userId, IEnumerable<int> sourceIdList,
        IEnumerable<UserSourceDto>? userSources)
    {
        foreach (var sourceId in sourceIdList)
        {
            if (!userSources.Any(a => a.SourceId == sourceId && !a.IsFollowing)) continue;

            var lastChapter = await _chapterRepository.GetSmallestChapterByMangaIdAsync(mangaId, sourceId);

            if (lastChapter == null) continue;

            UserManga userManga = new(userId, mangaId, sourceId, lastChapter.Id);

            await _userMangaRepository.CreateAsync(userManga);
        }
    }

    public async Task<IEnumerable<MangaUserLoggedDto>> GetUserMangasWithThreeLastChapterByUserId(string userId)
    {
        var userMangas = await _userMangaRepository.GetAllByUserIdAsync(userId);

        var userMangasByMangaId = userMangas
            .GroupBy(um => um.MangaId)
            .Select(group => new UserMangaGroupByManga(group.Select(us => us.Manga).FirstOrDefault()!,
                group.Select(um => new SourceWithLastChapterRead(um.SourceId, um.Source!.Name, um.CurrentChapterId))
                    .ToList()))
            .ToList();

        foreach (var userManga in userMangasByMangaId)
        {
            var chapters = await _chapterRepository.GetThreeLastByMangaIdAndSourceListAsync(userManga.Manga.Id,
                userManga.SourcesWithLastChapterRead.Select(sch => sch.SourceId).ToList());

            userManga.Manga.Chapters = chapters;
        }

        return _mapper.Map<IEnumerable<MangaUserLoggedDto>>(userMangasByMangaId);
    }

    public async Task DeleteUserMangasByMangaId(int mangaId, string userId)
    {
        await _userMangaRepository.DeleteAllByMangaIdAndUserIdAsync(mangaId, userId);
    }

    public async Task DeleteUserManga(int mangaId, string userId, int sourceId)
    {
        var userManga = await _userMangaRepository.GetByMangaIdUserIdAndSourceIdAsync(mangaId, userId, sourceId);

        if (userManga != null)
            await _userMangaRepository.DeleteAsync(userId, mangaId, sourceId);
    }
}