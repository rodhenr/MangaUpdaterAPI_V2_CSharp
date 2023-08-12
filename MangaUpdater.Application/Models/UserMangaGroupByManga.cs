using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Models;

public class UserMangaGroupByManga
{
    public Manga Manga { get; set; }
    public List<SourceWithLastChapterRead> SourcesWithLastChapterRead { get; set; }
}

public class SourceWithLastChapterRead
{
    public SourceWithLastChapterRead(int sourceId, string sourceName, int lastChapterRead)
    {
        SourceId = sourceId;
        SourceName = sourceName;
        LastChapterRead = lastChapterRead;
    }

    public int SourceId { get; set; }
    public string SourceName { get; set; }
    public int LastChapterRead { get; set; }
}