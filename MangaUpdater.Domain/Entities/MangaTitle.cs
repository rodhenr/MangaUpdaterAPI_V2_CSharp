using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class MangaTitle: Entity
{
    public required int MangaId { get; set; }
    public required string Name { get; set; }
    
    [JsonIgnore] public Manga? Manga { get; set; }
}