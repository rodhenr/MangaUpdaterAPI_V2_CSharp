using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class Genre : Entity
{
    public required string Name { get; set; }

    [JsonIgnore] public IEnumerable<MangaGenre>? MangaGenres { get; set; }
}