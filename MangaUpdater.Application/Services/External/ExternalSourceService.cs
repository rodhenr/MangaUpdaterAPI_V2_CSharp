using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.External;
using MangaUpdater.Application.Interfaces.External.MangaDex;
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

    public async Task UpdateChapters(MangaSource mangaSource, Source source, Chapter? lastChapter)
    {
        if (source.Name == "MangaDex")
        {
            var chapters = await _mangaDexApi.GetChaptersAsync(mangaSource.MangaId, mangaSource.SourceId, mangaSource.Url,
                source.BaseUrl, lastChapter?.Number);
            
            _chapterService.BulkCreate(chapters);
        }
        
        await _chapterService.SaveChanges();
    }
}