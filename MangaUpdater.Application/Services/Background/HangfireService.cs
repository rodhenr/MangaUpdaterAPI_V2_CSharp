﻿using Hangfire;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.Background;
using MangaUpdater.Application.Interfaces.External;

namespace MangaUpdater.Application.Services.Background;

public class HangfireService : IHangfireService
{
    private readonly IMangaService _mangaService;

    public HangfireService(IMangaService mangaService)
    {
        _mangaService = mangaService;
    }

    public async Task AddHangfireJobs()
    {
        var mangas = await _mangaService.GetMangasToUpdateChapters();

        var startTime = DateTime.Now;
        
        if (!mangas.Any())
        {
            ScheduleNextInvocation(startTime);
            return;
        }
        
        string? lastJobId = null;

        foreach (var manga in mangas)
        {
            lastJobId = BackgroundJob.Enqueue<IExternalSourceService>(job => job.UpdateChapters(manga));
        }

        BackgroundJob.ContinueJobWith<IHangfireService>(lastJobId, job => job.ScheduleNextInvocation(startTime));
    }

    public void ScheduleNextInvocation(DateTime startTime)
    {
        var timeDifference = DateTime.Now - startTime;

        BackgroundJob.Schedule<IHangfireService>(job => job.AddHangfireJobs(),
            timeDifference.Seconds >= 1800
                ? TimeSpan.FromSeconds(1)
                : TimeSpan.FromSeconds(1800 - timeDifference.TotalSeconds));
    }
}