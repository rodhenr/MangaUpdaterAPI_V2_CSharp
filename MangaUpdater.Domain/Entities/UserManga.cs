using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class UserManga
{
    public UserManga(string userId, int mangaId, int sourceId, int currentChapterId)
    {
        UserId = userId;
        MangaId = mangaId;
        SourceId = sourceId;
        CurrentChapterId = currentChapterId;
    }

    [MaxLength(450)]
    public string UserId { get; set; }

    public int MangaId { get; set; }

    public int SourceId { get; set; }

    public int CurrentChapterId { get; set; }

    [JsonIgnore]
    public Manga? Manga { get; set; }

    [JsonIgnore]
    public Source? Source { get; set; }
}
