﻿using MangaUpdater.Application.Interfaces;
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

    public async Task AddUserMangaBySourceIdList(int mangaId, int userId, IEnumerable<int> sourceIdList)
    {
        foreach (var sourceId in sourceIdList)
        {
            var lastChapter = await _chapterRepository.GetSmallestChapter(mangaId, sourceId);

            if (lastChapter != null)
            {
                UserManga userManga = new(userId, mangaId, sourceId, lastChapter.Id);

                await _userMangaRepository.CreateAsync(userManga);
            }
        }

        return;
    }
}
