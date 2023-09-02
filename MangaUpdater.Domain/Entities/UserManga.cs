using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class UserManga: Entity
{
    public required string UserId { get; set; }
    public required int MangaId { get; set; }
    public required int SourceId { get; set; }
    public required int CurrentChapterId { get; set; }

    [JsonIgnore] public Manga? Manga { get; set; }
    [JsonIgnore] public Source? Source { get; set; }
}