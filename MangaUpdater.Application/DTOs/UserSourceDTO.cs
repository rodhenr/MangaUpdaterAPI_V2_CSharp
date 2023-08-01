namespace MangaUpdater.Application.DTOs;
public class UserSourceDTO
{
    public UserSourceDTO(int sourceId, string sourceName, bool isFollowing)
    {
        SourceId = sourceId;
        SourceName = sourceName;
        IsFollowing = isFollowing;
    }

    public int SourceId { get; set; }

    public string SourceName { get; set; }

    public bool IsFollowing { get; set; }
}
