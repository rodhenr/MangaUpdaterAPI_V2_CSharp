using Hangfire;

namespace MangaUpdater.Application.Interfaces.Background;

public interface IHangfireService
{
    Task AddHangfireJobs();
    void ScheduleNextInvocation(DateTime dateJobStarted);
}