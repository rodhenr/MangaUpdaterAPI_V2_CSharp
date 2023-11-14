using MangaUpdater.Application.Interfaces.Background;
using MangaUpdater.Application.Interfaces.External;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MangaUpdater.Application.Services.Background;

public class MangaChapterUpdateService : BackgroundService
{
    private readonly IMangaUpdateQueue _updateQueue;
    private readonly IServiceProvider _serviceProvider;

    public MangaChapterUpdateService(IMangaUpdateQueue updateQueue, IServiceProvider serviceProvider)
    {
        _updateQueue = updateQueue;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var manga = await _updateQueue.DequeueAsync(stoppingToken);

            if (manga != null)
            {
                using var scope = _serviceProvider.CreateScope();
                var externalSourceService = scope.ServiceProvider.GetRequiredService<IExternalSourceService>();
                await externalSourceService.UpdateChapters(manga);
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}