namespace MangaUpdaterAPI.Models;

public class MangaGenre
{
    public MangaGenre(int mangaId, int genreId)
    {
        MangaId = mangaId;
        GenreId = genreId;
    }

    public int MangaId { get; set; }

    public int GenreId { get; set; }
}
