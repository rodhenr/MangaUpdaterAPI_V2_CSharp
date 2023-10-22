using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class UserChapter : Entity
{
    public required int UserMangaId { get; set; }
    public required int SourceId { get; set; }
    public int? ChapterId { get; set; }
    
    [JsonIgnore] public UserManga? UserManga { get; set; }
    [JsonIgnore] public Source? Source { get; set; }
    [JsonIgnore] public Chapter? Chapter { get; set; }
}