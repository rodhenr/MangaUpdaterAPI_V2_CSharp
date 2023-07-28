namespace MangaUpdater.Domain.Entities;

public class UserManga
{
    public UserManga(int userId, int mangaId, float lastChapter)
    {
        UserId = userId;
        MangaId = mangaId;
        LastChapter = lastChapter;
    }

    public int UserId { get; set; }

    public int MangaId { get; set; }

    public float LastChapter { get; set; }

    public Manga Manga { get; set; }
}
