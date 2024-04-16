using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Dtos;

public sealed class UserMangaGroupByMangaDto
{
    public UserMangaGroupByMangaDto(Manga manga, List<SourceWithLastChapterRead?> sourcesWithLastChapterRead)
    {
        Manga = manga;
        SourcesWithLastChapterRead = sourcesWithLastChapterRead;
    }

    public Manga Manga { get; set; }
    public List<SourceWithLastChapterRead> SourcesWithLastChapterRead { get; }
}

public sealed class SourceWithLastChapterRead
{
    public SourceWithLastChapterRead(int sourceId, int? lastChapterRead)
    {
        SourceId = sourceId;
        LastChapterRead = lastChapterRead;
    }

    public int SourceId { get; }
    public int? LastChapterRead { get; }
}