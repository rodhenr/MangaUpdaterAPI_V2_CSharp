using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class MangaGenre: Entity
{
    public required int MangaId { get; set; }
    public required int GenreId { get; set; }
    
    [JsonIgnore] public Manga? Manga { get; set; }
    [JsonIgnore] public Genre? Genre { get; set; }
}