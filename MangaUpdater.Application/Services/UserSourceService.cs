﻿using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class UserSourceService : IUserSourceService
{
    private readonly IUserMangaRepository _userMangaRepository;
    private readonly IMangaSourceRepository _mangaSourceRepository;

    public UserSourceService(IUserMangaRepository userMangaRepository, IMangaSourceRepository mangaSourceRepository)
    {
        _userMangaRepository = userMangaRepository;
        _mangaSourceRepository = mangaSourceRepository;
    }

    public async Task<IEnumerable<UserSourceDTO>> GetAllSourcesByMangaIdWithUserStatus(int mangaId, int userId)
    {
        var mangaSources = await _mangaSourceRepository.GetByMangaIdAsync(mangaId);
        var userMangas = await _userMangaRepository.GetByMangaIdAndUserIdAsync(mangaId, userId);

        var userSources = mangaSources.Select(a => new UserSourceDTO
        {
            SourceId = a.SourceId,
            SourceName = a.Source.Name,
            IsFollowing = userMangas.Any(b => b.SourceId == a.SourceId)
        }).ToList();

        return userSources;
    }
}