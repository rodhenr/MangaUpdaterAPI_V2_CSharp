using Hangfire;
using MangaUpdater.Core.Common.Extensions;
using MangaUpdater.Core.Features.Chapters;
using MangaUpdater.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Services;

public interface IHangfireService
{
    Task AddHangfireJobs(SourceEnum? sourceId);
    void ScheduleNextJob(DateTime dateJobStarted, SourceEnum? sourceId);
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

    public async Task AddHangfireJobs(SourceEnum? sourceId)
    {
        var mangasToUpdate = await GetMangasToUpdateChapters(sourceId);

        var startTime = DateTime.Now;
        
        if (mangasToUpdate.Count == 0)
        {
            ScheduleNextJob(startTime, null);
            return;
        }
        
        foreach (var mangas in mangasToUpdate)
        {
            var lastJobId = string.Empty;
            
            foreach (var mangaId in mangas.Value)
            {
                lastJobId = lastJobId == ""
                    ? _mediator.Enqueue(new UpdateChaptersCommand(mangaId, mangas.Key))
                    : EnqueueContinueJob(lastJobId, mangaId, mangas.Key);
            }
            
            BackgroundJob.ContinueJobWith<IHangfireService>(lastJobId, job => job.ScheduleNextJob(startTime, mangas.Key));
        }
    }
    
    public void ScheduleNextJob(DateTime startTime, SourceEnum? sourceId)
    {
        var timeDifference = DateTime.Now - startTime;

        BackgroundJob.Schedule<IHangfireService>(job => job.AddHangfireJobs(sourceId),
            timeDifference.Seconds >= 3600
                ? TimeSpan.FromSeconds(1)
                : TimeSpan.FromSeconds(3600 - timeDifference.TotalSeconds));
    }

    private async Task<Dictionary<SourceEnum, IEnumerable<int>>> GetMangasToUpdateChapters(SourceEnum? sourceId)
    {
        var queryable = _context.MangaSources.AsQueryable();

        if (sourceId is not null) queryable = queryable.Where(x => x.SourceId == (int)sourceId);

        return await queryable
            .GroupBy(x => x.SourceId)
            .ToDictionaryAsync(
                x => (SourceEnum)x.Key,
                x => x.Select(y => y.MangaId)
            );
    }

    private static string EnqueueContinueJob(string lastJobId, int mangaId, SourceEnum sourceId)
    {
        return BackgroundJob.ContinueJobWith(lastJobId, () => MediatorExtensions.Enqueue(new UpdateChaptersCommand(mangaId, sourceId)));
    }
}