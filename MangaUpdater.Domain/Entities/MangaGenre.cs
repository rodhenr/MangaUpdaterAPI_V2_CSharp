using System.Text.Json.Serialization;

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

    [JsonIgnore]
    public Manga Manga { get; set; }

    [JsonIgnore]
    public Genre Genre { get; set; }
}
