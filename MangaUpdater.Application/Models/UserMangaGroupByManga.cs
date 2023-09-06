using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Models;

public sealed class UserMangaGroupByManga
{
    public UserMangaGroupByManga(Manga manga, List<SourceWithLastChapterRead> sourcesWithLastChapterRead)
    {
        Manga = manga;
        SourcesWithLastChapterRead = sourcesWithLastChapterRead;
    }

    public Manga Manga { get; set; }
    public List<SourceWithLastChapterRead> SourcesWithLastChapterRead { get; set; }
}

public sealed class SourceWithLastChapterRead
{
    public SourceWithLastChapterRead(int sourceId, string sourceName, int? lastChapterRead)
    {
        SourceId = sourceId;
        SourceName = sourceName;
        LastChapterRead = lastChapterRead;
    }

    public int SourceId { get; set; }
    public string SourceName { get; set; }
    public int? LastChapterRead { get; set; }
}