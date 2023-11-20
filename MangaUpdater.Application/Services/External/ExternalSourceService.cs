using MangaUpdater.Application.Helpers;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.External;
using MangaUpdater.Application.Interfaces.External.AsuraScans;
using MangaUpdater.Application.Interfaces.External.MangaDex;
using MangaUpdater.Application.Models.External;

namespace MangaUpdater.Application.Services.External;

public class ExternalSourceService : IExternalSourceService
{
    private readonly IMangaDexApi _mangaDexApi;
    private readonly IAsuraScansApi _asuraScansApi;
    private readonly IChapterService _chapterService;

    public ExternalSourceService(IChapterService chapterService, IMangaDexApi mangaDexApi, IAsuraScansApi asuraScansApi)
    {
        _chapterService = chapterService;
        _mangaDexApi = mangaDexApi;
        _asuraScansApi = asuraScansApi;
    }

    public async Task UpdateChapters(MangaInfoToUpdateChapters mangaInfo)
    {
        switch (mangaInfo.SourceName)
        {
            case "MangaDex":
            {
                var chapters = await _mangaDexApi.GetChaptersAsync(mangaInfo.MangaId, mangaInfo.SourceId,
                    mangaInfo.MangaUrl,
                    mangaInfo.SourceBaseUrl, mangaInfo?.LastSavedChapter);

                _chapterService.BulkCreate(chapters.Distinct(new ChapterEqualityComparer()).ToList());
                break;
            }
            case "AsuraScans":
            {
                var chapters = await _asuraScansApi.GetChaptersAsync(mangaInfo.MangaId, mangaInfo.SourceId,
                    mangaInfo.MangaUrl,
                    mangaInfo.SourceBaseUrl, mangaInfo?.LastSavedChapter);

                _chapterService.BulkCreate(chapters.Distinct(new ChapterEqualityComparer()).ToList());
                break;
            }
        }

        await _chapterService.SaveChanges();
    }
}