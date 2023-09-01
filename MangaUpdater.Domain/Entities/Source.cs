using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class Source: Entity
{
    [MaxLength(50)] public required string Name { get; set; }
    [MaxLength(100)] public required string BaseUrl { get; set; }

    [JsonIgnore] public IEnumerable<UserManga>? UserMangas { get; set; }
    [JsonIgnore] public IEnumerable<MangaSource>? MangaSources { get; set; }
    [JsonIgnore] public IEnumerable<Chapter>? Chapters { get; set; }
}