using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class UserMangaChapterService : IUserMangaChapterService
{
    private readonly IUserMangaRepository _userMangaRepository;
    private readonly IChapterRepository _chapterRepository;
    private readonly IUserSourceService _userSourceService;
    private readonly IMapper _mapper;

    public UserMangaChapterService(IUserMangaRepository userMangaRepository,
        IChapterRepository chapterRepository,
        IUserSourceService userSourceService, IMapper mapper)
    {
        _userMangaRepository = userMangaRepository;
        _chapterRepository = chapterRepository;
        _userSourceService = userSourceService;
        _mapper = mapper;
    }

    public async Task AddUserMangaBySourceIdList(int mangaId, string userId, IEnumerable<int> sourceIdList)
    {
        var userSources = await _userSourceService.GetUserSourcesByMangaId(mangaId, userId);
        var sourceIdListToAdd = userSources?
            .Where(us => !us.IsFollowing && sourceIdList.Contains(us.SourceId))
            .Select(us => us.SourceId)
            .ToList();

        if (sourceIdListToAdd is null || !sourceIdListToAdd.Any()) return;

        foreach (var sourceId in sourceIdListToAdd)
        {
            await CreateUserManga(mangaId, sourceId, userId);
        }

        await _userMangaRepository.SaveAsync();
    }

    public async Task<IEnumerable<MangaUserLoggedDto>> GetUserMangasWithThreeLastChapterByUserId(string userId)
    {
        var userMangas = await _userMangaRepository.GetAllByUserIdAsync(userId);

        var userMangasByMangaId = userMangas
            .GroupBy(um => um.MangaId)
            .Select(group => new UserMangaGroupByMangaDto(group.Select(us => us.Manga).FirstOrDefault()!,
                group.Select(um => new SourceWithLastChapterRead(um.SourceId, um.Source!.Name, um.CurrentChapterId))
                    .ToList()))
            .ToList();

        foreach (var userManga in userMangasByMangaId)
        {
            var sourceList = userManga.SourcesWithLastChapterRead
                .Where(sch => sch.LastChapterRead is not null)
                .Select(sch => sch.SourceId)
                .ToList();
            var chapters =
                await _chapterRepository.GetThreeLastByMangaIdAndSourceListAsync(userManga.Manga.Id, sourceList);

            userManga.Manga.Chapters = chapters;
        }

        return _mapper.Map<List<MangaUserLoggedDto>>(userMangasByMangaId);
    }

    public async Task DeleteUserMangasByMangaId(int mangaId, string userId)
    {
        await _userMangaRepository.DeleteAsync(mangaId, userId);
    }

    public async Task DeleteUserMangaByMangaIdAndSourceId(int mangaId, int sourceId, string userId)
    {
        await _userMangaRepository.DeleteAsync(mangaId, sourceId, userId);
    }

    private async Task CreateUserManga(int mangaId, int sourceId, string userId)
    {
        var lastChapter = await _chapterRepository.GetSmallestChapterByMangaIdAsync(mangaId, sourceId);

        _userMangaRepository.Create(new UserManga
            { UserId = userId, MangaId = mangaId, SourceId = sourceId, CurrentChapterId = lastChapter?.Id });
    }
}