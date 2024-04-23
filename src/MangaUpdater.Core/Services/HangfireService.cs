using Hangfire;
using MangaUpdater.Core.Common.Extensions;
using MangaUpdater.Core.Features.Chapters;
using MangaUpdater.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Services;

public interface IHangfireService
{
    Task AddHangfireJobs();
    void ScheduleNextInvocation(DateTime dateJobStarted);
}

[RegisterScoped]
public class HangfireService : IHangfireService
{
    private readonly AppDbContextIdentity _context;
    private readonly IMediator _mediator;
    
    public HangfireService(AppDbContextIdentity context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task AddHangfireJobs()
    {
        var mangas = await _context.MangaSources
            .Select(x => new {x.MangaId, SourceId = (SourceEnum)x.SourceId})
            .ToListAsync();

        var startTime = DateTime.Now;
        
        if (mangas.Count == 0)
        {
            ScheduleNextInvocation(startTime);
            return;
        }
        
        var lastJobId = string.Empty;

        foreach (var manga in mangas)
        {
            lastJobId = _mediator.Enqueue(new UpdateChaptersCommand(manga.MangaId, manga.SourceId));
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