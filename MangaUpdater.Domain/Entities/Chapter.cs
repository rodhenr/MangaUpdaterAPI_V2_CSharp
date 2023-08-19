using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class Chapter
{
    public Chapter(int mangaId, int sourceId, DateTime date, float number)
    {
        MangaId = mangaId;
        SourceId = sourceId;
        Date = date;
        Number = number;
    }

    public int Id { get; set; }

    public int MangaId { get; set; }

    public int SourceId { get; set; }

    public DateTime Date { get; set; }

    public float Number { get; set; }

    [JsonIgnore]
    public Manga? Manga { get; set; }

    [JsonIgnore]
    public Source? Source { get; set; }
}
