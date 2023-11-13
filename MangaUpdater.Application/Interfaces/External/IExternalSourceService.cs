using MangaUpdater.Application.Models.External;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces.External;

public interface IExternalSourceService
{
    Task UpdateChapters(MangaInfoToUpdateChapters mangaInfo);
    Task UpdateAllChaptersFromMangaInfoList(IEnumerable<MangaInfoToUpdateChapters> mangaInfoList);
}