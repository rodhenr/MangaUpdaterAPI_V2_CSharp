using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public class MangaSource
{
    public MangaSource(int mangaId, int sourceId, string uRL)
    {
        MangaId = mangaId;
        SourceId = sourceId;
        URL = uRL;
    }

    public int MangaId { get; set; }

    public int SourceId { get; set; }

    [MaxLength(100)]
    public string URL { get; set; }

    [JsonIgnore]
    public Manga Manga { get; set; }

    [JsonIgnore]
    public Source Source { get; set; }
}
