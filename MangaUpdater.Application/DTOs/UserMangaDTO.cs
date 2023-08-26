namespace MangaUpdater.Application.DTOs;

public record UserMangaDTO
{
    public UserMangaDTO(string userId, int mangaId, int sourceId, int currentChapterId)
    {
        UserId = userId;
        MangaId = mangaId;
        SourceId = sourceId;
        CurrentChapterId = currentChapterId;
    }

    public string UserId { get; set; }
    public int MangaId { get; set; }
    public int SourceId { get; set; }
    public int CurrentChapterId { get; set; }
}
