namespace MangaUpdaterAPI.Models;

public class Genre
{
    public Genre(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
}
