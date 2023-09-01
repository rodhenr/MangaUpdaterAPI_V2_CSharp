using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class MangaSource: Entity
{
    public MangaSource(int mangaId, int sourceId, string url)
    {
        MangaId = mangaId;
        SourceId = sourceId;
        Url = url;
    }

    public int MangaId { get; set; }
    public int SourceId { get; set; }
    [MaxLength(100)] public string Url { get; set; }
    
    [JsonIgnore] public Manga? Manga { get; set; }
    [JsonIgnore] public Source? Source { get; set; }
}