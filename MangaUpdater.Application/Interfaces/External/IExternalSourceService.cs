using MangaUpdater.Application.Models.External;

namespace MangaUpdater.Application.Interfaces.External;

public interface IExternalSourceService
{
    Task UpdateChapters(MangaInfoToUpdateChapters mangaInfo);
}