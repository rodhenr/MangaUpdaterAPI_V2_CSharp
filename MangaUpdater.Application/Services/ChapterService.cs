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

    public async Task BulkCreate(int mangaId, int sourceId, Dictionary<string, string> chapters)
    {
        foreach (var chapter in chapters)
        {
            var newChapter = new Chapter(mangaId, sourceId, DateTime.Parse(chapter.Value), float.Parse(chapter.Key));
            await AddChapter(newChapter);
        }
    }

    public async Task<Chapter?> GetChapterById(int id)
    {
        return await _chapterRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Chapter>> GetChaptersByMangaId(int mangaId, int? max)
    {
        return await _chapterRepository.GetAllByMangaIdAsync(mangaId, max ?? 0);
    }
}
