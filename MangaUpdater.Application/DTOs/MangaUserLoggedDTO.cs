namespace MangaUpdater.Application.DTOs;

public class MangaUserLoggedDTO
{
    public int Id { get; set; }

    public string CoverURL { get; set; }

    public string Name { get; set; }

    public IEnumerable<ChapterDTO>? Chapters { get; set; }
}
