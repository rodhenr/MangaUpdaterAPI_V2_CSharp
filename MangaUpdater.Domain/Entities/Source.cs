using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class Source
{
    public Source(string name, string baseUrl)
    {
        Name = name;
        BaseUrl = baseUrl;
    }

    public int Id { get; set; }

    [MaxLength(50)] public string Name { get; set; }

    [MaxLength(100)] public string BaseUrl { get; set; }

    [JsonIgnore] public ICollection<UserManga>? UserMangas { get; set; }

    [JsonIgnore] public ICollection<MangaSource>? MangaSources { get; set; }

    [JsonIgnore] public ICollection<Chapter>? Chapters { get; set; }
}