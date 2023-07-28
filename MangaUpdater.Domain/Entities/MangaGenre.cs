namespace MangaUpdater.Domain.Entities;

public class MangaGenre
{
    public MangaGenre(int mangaId, int genreId)
    {
        MangaId = mangaId;
        GenreId = genreId;
    }

    public int MangaId { get; set; }

    public int GenreId { get; set; }

    public Manga Manga { get; set; }
    public Genre Genre { get; set; }
}
