using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class Manga : Entity
{
    public Manga(string coverUrl, string name, string alternativeName, string author, string synopsis, string type,
        int myAnimeListId)
    {
        CoverUrl = coverUrl;
        Name = name;
        AlternativeName = alternativeName;
        Author = author;
        Synopsis = synopsis;
        Type = type;
        MyAnimeListId = myAnimeListId;
    }

    [MaxLength(200)] public string CoverUrl { get; set; }
    [MaxLength(200)] public string Name { get; set; }
    [MaxLength(200)] public string AlternativeName { get; set; }
    [MaxLength(50)] public string Author { get; set; }
    [MaxLength(2000)] public string Synopsis { get; set; }
    [MaxLength(20)] public string Type { get; set; }
    public int MyAnimeListId { get; set; }
    
    [JsonIgnore] public IEnumerable<Chapter>? Chapters { get; set; }
    [JsonIgnore] public IEnumerable<UserManga>? UserMangas { get; set; }
    [JsonIgnore] public IEnumerable<MangaGenre>? MangaGenres { get; set; }
    [JsonIgnore] public IEnumerable<MangaSource>? MangaSources { get; set; }
}