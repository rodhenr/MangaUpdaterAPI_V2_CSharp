namespace MangaUpdater.Application.DTOs;

public record MangaDTO
{
    public MangaDTO(string coverURL, string name, string alternativeName, string author, string synopsis, string type, int myAnimeListId, bool isUserFollowing, IEnumerable<string>? genres, IEnumerable<SourceDTO>? sources, IEnumerable<ChapterDTO>? chapters)
    {
        CoverURL = coverURL;
        Name = name;
        AlternativeName = alternativeName;
        Author = author;
        Synopsis = synopsis;
        Type = type;
        MyAnimeListId = myAnimeListId;
        IsUserFollowing = isUserFollowing;
        Genres = genres;
        Sources = sources;
        Chapters = chapters;
    }

    public string CoverURL { get; set; }

    public string Name { get; set; }

    public string AlternativeName { get; set; }

    public string Author { get; set; }

    public string Synopsis { get; set; }

    public string Type { get; set; }

    public int MyAnimeListId { get; set; }

    public bool IsUserFollowing { get; set; }

    public IEnumerable<string>? Genres { get; set; }

    public IEnumerable<SourceDTO>? Sources { get; set; }

    public IEnumerable<ChapterDTO>? Chapters { get; set; }
}
