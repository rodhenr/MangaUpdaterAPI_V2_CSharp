﻿using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class ChapterService : IChapterService
{
    private readonly IChapterRepository _chapterRepository;

    public ChapterService(IChapterRepository chapterRepository)
    {
        _chapterRepository = chapterRepository;        
    }

    public async Task AddChapter(Chapter chapter)
    {
        await _chapterRepository.CreateAsync(chapter);
    }

    public async Task<IEnumerable<Chapter>> GetChaptersByMangaId(int mangaId, int? max)
    {
        return await _chapterRepository.GetChaptersByIdAsync(mangaId, max ?? 0);
    }
}