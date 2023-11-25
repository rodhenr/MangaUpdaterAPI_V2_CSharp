using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Exceptions;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class UserMangaChapterService : IUserMangaChapterService
{
    private readonly IUserMangaRepository _userMangaRepository;
    private readonly IChapterRepository _chapterRepository;
    private readonly IUserSourceService _userSourceService;
    private readonly IUserChapterRepository _userChapterRepository;

    public UserMangaChapterService(IUserMangaRepository userMangaRepository, IChapterRepository chapterRepository,
        IUserSourceService userSourceService, IUserChapterRepository userChapterRepository)
    {
        _userMangaRepository = userMangaRepository;
        _chapterRepository = chapterRepository;
        _userSourceService = userSourceService;
        _userChapterRepository = userChapterRepository;
    }

    public async Task AddUserMangaBySourceIdList(int mangaId, string userId, IEnumerable<int> sourceIdList)
    {
        var userManga = await _userMangaRepository.GetByMangaIdAndUserIdAsync(mangaId, userId);

        if (userManga is null)
        {
            _userMangaRepository.Create(new UserManga { MangaId = mangaId, UserId = userId });
            await _userChapterRepository.SaveAsync();
        }

        var userSources = await _userSourceService.GetUserSourcesByMangaId(mangaId, userManga?.Id);

        var userSourceList = userSources.ToList();

        var sourceIdListToAdd = userSourceList
            .Where(us => !us.IsFollowing && sourceIdList.Contains(us.SourceId))
            .Select(us => us.SourceId)
            .ToList();

        var sourceIdListToRemove = userSourceList
            .Where(us => us.IsFollowing && !sourceIdList.Contains(us.SourceId))
            .Select(us => us.SourceId)
            .ToList();

        userManga ??= await _userMangaRepository.GetByMangaIdAndUserIdAsync(mangaId, userId);

        if (userManga is null) return;

        if (sourceIdListToAdd.Any())
        {
            foreach (var sourceId in sourceIdListToAdd)
            {
                _userChapterRepository.Create(new UserChapter { UserMangaId = userManga.Id, SourceId = sourceId });
            }

            await _userChapterRepository.SaveAsync();
        }

        if (sourceIdListToRemove.Any())
            await _userChapterRepository.DeleteRangeAsync(userManga.Id, sourceIdListToRemove);
    }

    public async Task<IEnumerable<MangaUserLoggedDto>> GetUserMangasWithThreeLastChapterByUserId(string userId,
        int page, int limit)
    {
        var userMangas = await _userMangaRepository.GetAllByUserIdWithPageLimitAsync(userId, page, limit);

        var userMangasFiltered = userMangas
            .Select(um => new MangaUserLoggedDto
            {
                Id = um.MangaId,
                CoverUrl = um.Manga!.CoverUrl,
                Name = um.Manga!.MangaTitles!.First(mt => mt.IsMainTitle).Name,
                Chapters = um.Manga!.Chapters!.OrderByDescending(ch => ch.Date).Take(3).Select(ch =>
                {
                    var userChapter =
                        um.UserChapter!.FirstOrDefault(uc => uc.SourceId == ch.SourceId && uc.ChapterId is not null);
                    return new ChapterDto
                    {
                        ChapterId = ch.Id,
                        SourceId = ch.SourceId,
                        SourceName = ch.Source!.Name,
                        Date = ch.Date,
                        Number = ch.Number,
                        IsUserAllowedToRead = true,
                        Read = userChapter is not null &&
                               float.Parse(userChapter.Chapter!.Number) >= float.Parse(ch.Number)
                    };
                })
            });

        return userMangasFiltered;
    }

    public async Task DeleteUserMangasByMangaId(int mangaId, string userId)
    {
        var userManga = await _userMangaRepository.GetByMangaIdAndUserIdAsync(mangaId, userId);

        if (userManga is not null) await _userChapterRepository.DeleteAsync(userManga.Id);

        await _userMangaRepository.DeleteAsync(mangaId, userId);
    }

    public async Task DeleteUserMangaByMangaIdAndSourceId(int mangaId, int sourceId, string userId)
    {
        var userManga = await _userMangaRepository.GetByMangaIdAndUserIdAsync(mangaId, userId);

        if (userManga is not null) await _userChapterRepository.DeleteAsync(userManga.Id, sourceId);
    }

    public async Task UpdateOrCreateUserChapter(string userId, int mangaId, int sourceId, int chapterId)
    {
        var userManga = await _userMangaRepository.GetByMangaIdAndUserIdAsync(mangaId, userId);
        if (userManga is null) throw new BadRequestException("User not following this manga/source");

        var chapter = await _chapterRepository.GetByIdAndMangaIdAsync(mangaId, chapterId);
        if (chapter is null) throw new BadRequestException("Chapter not found");

        var userChapter = await _userChapterRepository.GetByUserMangaIdAndSourceIdAsync(userManga.Id, sourceId);

        if (userChapter != null)
        {
            if (userChapter.ChapterId == chapterId)
            {
                var previousChapter = await _chapterRepository.GetPreviousChapterAsync(mangaId, sourceId, chapterId);
                userChapter.ChapterId = previousChapter?.Id;
            }
            else
            {
                userChapter.ChapterId = chapterId;
            }
        }

        await _userChapterRepository.SaveAsync();
    }
}