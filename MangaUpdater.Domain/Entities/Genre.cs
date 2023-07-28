using System.ComponentModel.DataAnnotations;

namespace MangaUpdater.Domain.Entities;

public class Genre
{
    public Genre(string name)
    {
        Name = name;
    }

    public int Id { get; set; }

    [MaxLength(20)]
    public string Name { get; set; }
    
    public ICollection<MangaGenre> MangaGenres { get; set; }
}
