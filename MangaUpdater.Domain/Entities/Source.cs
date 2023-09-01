using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class Source: Entity
{
    public Source(string name, string baseUrl)
    {
        Name = name;
        BaseUrl = baseUrl;
    }

    [MaxLength(50)] public string Name { get; set; }
    [MaxLength(100)] public string BaseUrl { get; set; }

    [JsonIgnore] public IEnumerable<UserManga>? UserMangas { get; set; }
    [JsonIgnore] public IEnumerable<MangaSource>? MangaSources { get; set; }
    [JsonIgnore] public IEnumerable<Chapter>? Chapters { get; set; }
}