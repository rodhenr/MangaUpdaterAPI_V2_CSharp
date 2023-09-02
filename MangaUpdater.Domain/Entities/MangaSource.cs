using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class MangaSource: Entity
{
    public required int MangaId { get; set; }
    public required int SourceId { get; set; }
    public required string Url { get; set; }
    
    [JsonIgnore] public Manga? Manga { get; set; }
    [JsonIgnore] public Source? Source { get; set; }
}