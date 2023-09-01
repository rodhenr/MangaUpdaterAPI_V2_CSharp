using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class Manga : Entity
{
    [MaxLength(200)] public required string CoverUrl { get; set; }
    [MaxLength(200)] public required string Name { get; set; }
    [MaxLength(200)] public required string AlternativeName { get; set; }
    [MaxLength(50)] public required string Author { get; set; }
    [MaxLength(2000)] public required string Synopsis { get; set; }
    [MaxLength(20)] public required string Type { get; set; }
    public required int MyAnimeListId { get; set; }

    [JsonIgnore] public IEnumerable<Chapter>? Chapters { get; set; }
    [JsonIgnore] public IEnumerable<UserManga>? UserMangas { get; set; }
    [JsonIgnore] public IEnumerable<MangaGenre>? MangaGenres { get; set; }
    [JsonIgnore] public IEnumerable<MangaSource>? MangaSources { get; set; }
}