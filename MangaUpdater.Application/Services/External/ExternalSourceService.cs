using MangaUpdater.Application.Helpers;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.External;
using MangaUpdater.Application.Interfaces.External.MangaDex;
using MangaUpdater.Application.Models.External;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Services.External;

public class ExternalSourceService : IExternalSourceService
{
    private readonly IMangaDexApi _mangaDexApi;
    private readonly IChapterService _chapterService;

    public ExternalSourceService(IChapterService chapterService, IMangaDexApi mangaDexApi)
    {
        _chapterService = chapterService;
        _mangaDexApi = mangaDexApi;
    }

    public async Task UpdateChapters(MangaInfoToUpdateChapters mangaInfo)
    {
        if (mangaInfo.SourceName == "MangaDex")
        {
            var chapters = await _mangaDexApi.GetChaptersAsync(mangaInfo.MangaId, mangaInfo.SourceId, mangaInfo.MangaUrl,
                mangaInfo.SourceBaseUrl, mangaInfo?.LastSavedChapter);
            
            _chapterService.BulkCreate(chapters.Distinct(new ChapterEqualityComparer()).ToList());
        }
        
        await _chapterService.SaveChanges();
    }

    public Task UpdateAllChaptersFromMangaInfoList(IEnumerable<MangaInfoToUpdateChapters> mangaInfoList)
    {
        throw new NotImplementedException();
    }
}