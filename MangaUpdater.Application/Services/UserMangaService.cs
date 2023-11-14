using System.Globalization;
using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Helpers;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Models.External;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class UserMangaService : IUserMangaService
{
    private readonly IUserMangaRepository _userMangaRepository;
    private readonly IMapper _mapper;

    public UserMangaService(IMapper mapper, IUserMangaRepository userMangaRepository)
    {
        _userMangaRepository = userMangaRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MangaUserDto>> GetMangasByUserId(string userId)
    {
        var mangas = await _userMangaRepository.GetAllByUserIdAsync(userId);

        return _mapper.Map<IEnumerable<MangaUserDto>>(mangas.Select(userManga => userManga.Manga));
    }

    public async Task<UserManga?> GetByUserIdAndMangaId(string userId, int mangaId)
    {
        return await _userMangaRepository.GetByMangaIdAndUserIdAsync(mangaId, userId);
    }

    public async Task Update(UserManga userManga)
    {
        _userMangaRepository.Update(userManga);
        await _userMangaRepository.SaveChangesAsync(userManga);
    }

    public async Task<List<MangaInfoToUpdateChapters>> GetMangasToUpdateChapters(string userId)
    {
        var userMangaList = await _userMangaRepository.GetMangasToUpdateChaptersAsync(userId);

        var mangasToUpdateChapters = userMangaList
            .Where(um => um.UserChapter is not null && um.Manga?.MangaSources is not null)
            .Select(um => new MangaInfoToUpdateChapters(um.MangaId, um.UserChapter!.SourceId,
                um.Manga!.MangaSources!.First(ms => ms.MangaId == um.MangaId && ms.SourceId == um.UserChapter.SourceId)
                    .Url, um.UserChapter!.Source!.BaseUrl, um.UserChapter.Source.Name,
                um.Manga?.Chapters?.First().Number))
            .ToList();

        return mangasToUpdateChapters;
    }
}