namespace MangaUpdater.Domain.Entities;

public class UserManga
{
    public UserManga(int userId, int mangaId, int sourceId, float lastChapter, int chapterId)
    {
        UserId = userId;
        MangaId = mangaId;
        SourceId = sourceId;
        LastChapter = lastChapter;
        ChapterId = chapterId;
    }

    public int UserId { get; set; }

    public int MangaId { get; set; }

    public int SourceId { get; set; }
    public float LastChapter { get; set; }

    public int ChapterId { get; set; }

    public User User { get; set; }
    public Manga Manga { get; set; }
    public Source Source { get; set; }
    public Chapter Chapter { get; set; }
}
