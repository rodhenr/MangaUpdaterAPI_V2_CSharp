namespace MangaUpdater.Application.DTOs;

public class ChapterDTO
{
    public int ChapterId { get; set; }

    public int SourceId { get; set; }
    public string SourceName { get; set; }

    public DateTime Date { get; set; }

    public float Number { get; set; }

    public bool Read { get; set; }
}
