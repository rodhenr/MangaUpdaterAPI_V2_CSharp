using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using MangaUpdater.Core.Common.Extensions;
using MangaUpdater.Core.Features.External;
using MediatR;
using MangaUpdater.Core.Models;
using MangaUpdater.Data;

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
        var mangaList = await _context.Mangas
            .AsNoTracking()
            .Where(m => m.MangaSources!.Any())
            .Include(m => m.Chapters)
            .Include(m => m.MangaSources)!
            .ThenInclude(ms => ms.Source)
            .ToListAsync();

        var mangasToUpdateChapters = mangaList
            .SelectMany(m => m.MangaSources!, (m, ms) => {
                var lastChapter = m.Chapters!
                    .Where(ch => ch.SourceId == ms.SourceId)
                    .MaxBy(ch => float.Parse(ch.Number, CultureInfo.InvariantCulture));

                return new MangaInfoToUpdateModel(m.Id, ms.SourceId, ms.Url, ms.Source!.BaseUrl, ms.Source.Name, lastChapter is null ? "0" : lastChapter.Number); })
            .ToList();

        var startTime = DateTime.Now;
        
        if (mangasToUpdateChapters.Count == 0)
        {
            ScheduleNextInvocation(startTime);
            throw new Exception(""); // Remove
        }
        
        string? lastJobId = null;

        foreach (var manga in mangasToUpdateChapters)
        {
            //lastJobId = 
            //_mediator.Enqueue("UpdateManga", new UpdateChaptersFromAsuraScansCommand(manga.MangaId));
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