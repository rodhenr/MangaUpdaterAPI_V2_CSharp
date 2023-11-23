using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class UserManga: Entity
{
    public required string UserId { get; set; }
    public required int MangaId { get; set; }
    
    [JsonIgnore] public Manga? Manga { get; set; }
    [JsonIgnore] public IEnumerable<UserChapter>? UserChapter { get; set; }
}