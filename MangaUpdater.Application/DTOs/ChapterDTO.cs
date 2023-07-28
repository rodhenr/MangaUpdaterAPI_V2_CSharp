namespace MangaUpdater.Application.DTOs;

public class ChapterDTO
{
    public ChapterDTO(string sourceName, DateTime date, float number)
    {
        SourceName = sourceName;
        Date = date;
        Number = number;
    }

    public string SourceName { get; set; }
    public DateTime Date { get; set; }

    public float Number { get; set; }
}
