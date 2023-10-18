using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.External.MangaLivre;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Services.External.MangaLivre;

public class MangaLivreService : IMangaLivreService
{
    private readonly IMangaLivreApi _mangaLivreApi;
    private readonly IChapterService _chapterService;
    private readonly IMangaSourceService _mangaSourceService;

    public MangaLivreService(IMangaLivreApi mangaLivreApi, IChapterService chapterService,
        IMangaSourceService mangaSourceService)
    {
        _mangaLivreApi = mangaLivreApi;
        _chapterService = chapterService;
        _mangaSourceService = mangaSourceService;
    }

    public async Task RegisterSourceAndChapters(int mangaId, int sourceId, string url)
    {
        _mangaSourceService.Add(new MangaSource { MangaId = mangaId, SourceId = sourceId, Url = url });
        await UpdateChapters(mangaId, sourceId, "0", url);
    }

    public async Task UpdateChapters(int mangaId, int sourceId, string lastSavedChapterNumber, string url)
    {
        var chapters = await _mangaLivreApi.GetChaptersAsync(int.Parse(url), lastSavedChapterNumber);

        var chapterList = chapters
            .Select(ch => new Chapter()
            {
                MangaId = mangaId,
                SourceId = sourceId,
                Number = ch.ChapterNumber,
                Date = DateTime.Parse(ch.ChapterDate)
            })
            .ToList();

        _chapterService.BulkCreate(chapterList);
        
        await _mangaSourceService.SaveChanges();
    }
}