using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class Genre : Entity
{
    [MaxLength(20)] public required string Name { get; set; }
    
    [JsonIgnore] public IEnumerable<MangaGenre>? MangaGenres { get; set; }
}