using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class Genre : Entity
{
    public Genre(string name)
    {
        Name = name;
    }

    [MaxLength(20)] public string Name { get; set; }
    
    [JsonIgnore] public IEnumerable<MangaGenre>? MangaGenres { get; set; }
}