namespace MangaUpdaterAPI.Models;

public class Chapter
{
    public Chapter(int id, int mangaId, int sourceId, DateTime date, string number)
    {
        Id = id;
        MangaId = mangaId;
        SourceId = sourceId;
        Date = date;
        Number = number;
    }

    public int Id { get; set; }
    public int MangaId { get; set; }
    public int SourceId { get; set; }
    public DateTime Date { get; set; }
    public string Number { get; set; }
}
