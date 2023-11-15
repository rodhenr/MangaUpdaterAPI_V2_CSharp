using Hangfire;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.Background;
using MangaUpdater.Application.Interfaces.External;

namespace MangaUpdater.Application.Services.Background;

public class HangfireService : IHangfireService
{
    private readonly IMangaService _mangaService;
    private readonly IExternalSourceService _externalSourceService;

    public HangfireService(IMangaService mangaService, IExternalSourceService externalSourceService)
    {
        _mangaService = mangaService;
        _externalSourceService = externalSourceService;
    }

    public async Task AddHangfireJobs()
    {
        var mangas = await _mangaService.GetMangasToUpdateChapters();

        foreach (var manga in mangas)
        {
            RecurringJob.AddOrUpdate($"JobForMangaId_{manga.MangaId}_SourceId_{manga.SourceId}",
                () => _externalSourceService.UpdateChapters(manga), "*/36 * * * *");
        }
    }
}