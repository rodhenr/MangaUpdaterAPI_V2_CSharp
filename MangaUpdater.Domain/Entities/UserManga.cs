using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class UserManga
{
    public UserManga(int userId, int mangaId, int sourceId, int currentChapterId)
    {
        UserId = userId;
        MangaId = mangaId;
        SourceId = sourceId;
        CurrentChapterId = currentChapterId;
    }

    public int UserId { get; set; }

    public int MangaId { get; set; }

    public int SourceId { get; set; }

    public int CurrentChapterId { get; set; }


    [JsonIgnore]
    public User? User { get; set; }

    [JsonIgnore]
    public Manga? Manga { get; set; }

    [JsonIgnore]
    public Source? Source { get; set; }
}
