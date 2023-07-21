namespace MangaUpdaterAPI.Models;

public class UserManga
{
    public UserManga(int userId, int mangaId, string lastChapter)
    {
        UserId = userId;
        MangaId = mangaId;
        LastChapter = lastChapter;
    }

    public int UserId { get; set; }
    public int MangaId { get; set; }
    public string LastChapter { get; set; }
}
