using MangaUpdater.Application.Interfaces.Background;
using MangaUpdater.Application.Models.External;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Services.Background;

public class MangaUpdateQueue : IMangaUpdateQueue
{
    private readonly Queue<MangaInfoToUpdateChapters?> _queue = new();
    private readonly object _lock = new();

    public Task EnqueueAsync(MangaInfoToUpdateChapters manga)
    {
        lock (_lock)
        {
            _queue.Enqueue(manga);
        }

        return Task.CompletedTask;
    }

    public Task<MangaInfoToUpdateChapters?> DequeueAsync(CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            if (_queue.Count > 0)
            {
                return Task.FromResult(_queue.Dequeue());
            }
        }

        return Task.FromResult<MangaInfoToUpdateChapters>(null);
    }
}