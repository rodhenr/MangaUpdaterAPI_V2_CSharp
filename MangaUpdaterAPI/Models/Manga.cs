namespace MangaUpdaterAPI.Models;

public class Manga
{
    public Manga(int id, string coverURL, string name, string alternativeName, string author, string synopsis, string type, string myAnimeListURL)
    {
        Id = id;
        CoverURL = coverURL;
        Name = name;
        AlternativeName = alternativeName;
        Author = author;
        Synopsis = synopsis;
        Type = type;
        MyAnimeListURL = myAnimeListURL;
    }

    public int Id { get; set; }
    public string CoverURL { get; set; }
    public string Name { get; set; }
    public string AlternativeName { get; set; }
    public string Author { get; set; }
    public string Synopsis { get; set; }
    public string Type { get; set; }
    public string MyAnimeListURL { get; set; }

}
