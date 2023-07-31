namespace MangaUpdater.Application.DTOs;

public class ChapterDTO
{
    public ChapterDTO(int chapterId, int sourceId, string sourceName, DateTime date, float number, bool read)
    {
        ChapterId = chapterId;
        SourceId = sourceId;
        SourceName = sourceName;
        Date = date;
        Number = number;
        Read = read;
    }

    public int ChapterId { get; set; }

    public int SourceId { get; set; }
    public string SourceName { get; set; }

    public DateTime Date { get; set; }

    public float Number { get; set; }

    public bool Read { get; set; }
}
