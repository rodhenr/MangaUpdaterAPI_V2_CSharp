using System.ComponentModel.DataAnnotations;

namespace MangaUpdaterAPI.Models;

public class Genre
{
    public Genre(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }

    [MaxLength(20)]
    public string Name { get; set; }
}
