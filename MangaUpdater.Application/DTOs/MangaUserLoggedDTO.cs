namespace MangaUpdater.Application.DTOs;

public record MangaUserLoggedDTO
{
    public MangaUserLoggedDTO(int id, string coverURL, string name, IEnumerable<ChapterDTO>? chapters)
    {
        Id = id;
        CoverURL = coverURL;
        Name = name;
        Chapters = chapters;
    }

    public int Id { get; set; }

    public string CoverURL { get; set; }

    public string Name { get; set; }

    public IEnumerable<ChapterDTO>? Chapters { get; set; }
}
