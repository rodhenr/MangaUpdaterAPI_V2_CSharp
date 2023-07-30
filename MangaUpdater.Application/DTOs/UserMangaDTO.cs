namespace MangaUpdater.Application.DTOs;

public class UserMangaDTO
{
    public UserMangaDTO(int userId, int mangaId, int sourceId, int currentChapterId)
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
}
