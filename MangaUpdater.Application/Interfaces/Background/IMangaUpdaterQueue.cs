using MangaUpdater.Application.Models.External;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces.Background;

public interface IMangaUpdateQueue
{
    Task EnqueueAsync(MangaInfoToUpdateChapters manga);
    Task<MangaInfoToUpdateChapters?> DequeueAsync(CancellationToken cancellationToken);
}