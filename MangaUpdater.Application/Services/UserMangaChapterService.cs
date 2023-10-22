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
    private readonly IMapper _mapper;

    public UserMangaChapterService(IUserMangaRepository userMangaRepository,
        IChapterRepository chapterRepository,
        IUserSourceService userSourceService, IMapper mapper, IUserChapterRepository userChapterRepository)
    {
        _userMangaRepository = userMangaRepository;
        _chapterRepository = chapterRepository;
        _userSourceService = userSourceService;
        _mapper = mapper;
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

        if (!sourceIdListToAdd.Any()) return;

        userManga ??= await _userMangaRepository.GetByMangaIdAndUserIdAsync(mangaId, userId);

        if (userManga is null) return;

        foreach (var sourceId in sourceIdListToAdd)
        {
            _userChapterRepository.Create(new UserChapter { UserMangaId = userManga.Id, SourceId = sourceId });
        }

        await _userChapterRepository.SaveAsync();
    }

    public async Task<IEnumerable<MangaUserLoggedDto>> GetUserMangasWithThreeLastChapterByUserId(string userId)
    {
        var userMangas = await _userMangaRepository.GetAllByUserIdAsync(userId);

        var userMangasByMangaId = userMangas
            .Where(um => um.UserChapter is not null)
            .GroupBy(um => um.MangaId)
            .Select(group => new UserMangaGroupByMangaDto(group.Select(us => us.Manga).FirstOrDefault()!,
                group.Select(um =>
                    {
                        if (um.UserChapter != null)
                            return new SourceWithLastChapterRead(um.UserChapter.SourceId, um.UserChapter.Source!.Name,
                                um.UserChapter!.ChapterId);

                        return null;
                    })
                    .ToList()))
            .ToList();

        foreach (var userManga in userMangasByMangaId)
        {
            var sourceList = userManga.SourcesWithLastChapterRead
                .Select(sch => sch.SourceId)
                .ToList();
            
            var chapters =
                await _chapterRepository.GetThreeLastByMangaIdAndSourceListAsync(userManga.Manga.Id, sourceList);

            userManga.Manga.Chapters = chapters;
        }

        userMangasByMangaId.Sort((x, y) =>
        {
            if (y.Manga.Chapters != null)
                return x.Manga.Chapters != null
                    ? y.Manga.Chapters.FirstOrDefault()!.Date.CompareTo(x.Manga.Chapters.FirstOrDefault()!.Date)
                    : 1;
            return 0;
        });

        return _mapper.Map<List<MangaUserLoggedDto>>(userMangasByMangaId);
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

        if (userManga is null) return;

        await _userChapterRepository.DeleteAsync(userManga.Id, sourceId);
    }

    public async Task UpdateOrCreateUserChapter(string userId, int mangaId, int sourceId, int chapterId)
    {
        var userManga = await _userMangaRepository.GetByMangaIdAndUserIdAsync(mangaId, userId);
        if (userManga is null) throw new BadRequestException("User not following this manga/source");

        var chapter = await _chapterRepository.GetByIdAndMangaIdAsync(mangaId, chapterId);
        if (chapter is null) throw new BadRequestException("Chapter not found");

        var userChapter = await _userChapterRepository.GetByUserMangaIdAndSourceIdAsync(userManga.Id, sourceId);

        if (userChapter != null) userChapter.ChapterId = chapterId;

        await _userChapterRepository.SaveAsync();
    }
}