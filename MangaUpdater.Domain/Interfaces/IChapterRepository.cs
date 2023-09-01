﻿using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IChapterRepository : IBaseRepository<Chapter>
{
    void BulkCreateAsync(IEnumerable<Chapter> chapters);
    Task<IEnumerable<Chapter>> GetByMangaIdAsync(int mangaId, int max = 0);
    Task<Chapter?> GetSmallestChapterByMangaIdAsync(int mangaId, int sourceId);
    Task<IEnumerable<Chapter>> GetThreeLastByMangaIdAndSourceListAsync(int mangaId, List<int> sourceList);
    Task<IEnumerable<float>> GetChaptersNumberByMangaIdAndSourceIdAsync(int mangaId, int sourceId);
}