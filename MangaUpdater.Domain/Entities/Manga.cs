using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public class Manga
{
    public Manga(string coverURL, string name, string alternativeName, string author, string synopsis, string type, string myAnimeListURL)
    {
        CoverURL = coverURL;
        Name = name;
        AlternativeName = alternativeName;
        Author = author;
        Synopsis = synopsis;
        Type = type;
        MyAnimeListURL = myAnimeListURL;
    }

    public int Id { get; set; }

    [MaxLength(200)]
    public string CoverURL { get; set; }

    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(200)]
    public string AlternativeName { get; set; }

    [MaxLength(50)]
    public string Author { get; set; }

    [MaxLength(2000)]
    public string Synopsis { get; set; }

    [MaxLength(20)]
    public string Type { get; set; }

    [MaxLength(200)]
    public string MyAnimeListURL { get; set; }

    [JsonIgnore]
    public ICollection<Chapter> Chapters { get; set; }

    [JsonIgnore]
    public ICollection<UserManga> UserMangas { get; set; }

    [JsonIgnore]
    public ICollection<MangaGenre> MangaGenres { get; set; }

    [JsonIgnore]
    public ICollection<MangaSource> MangaSources { get; set; }
}
