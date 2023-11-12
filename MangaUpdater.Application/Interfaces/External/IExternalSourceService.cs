using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces.External;

public interface IExternalSourceService
{
    Task UpdateChapters(MangaSource mangaSource, Source source, Chapter lastChapter);
}