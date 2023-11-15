namespace MangaUpdater.Application.Interfaces.Background;

public interface IHangfireService
{
    Task AddHangfireJobs();
}