namespace MangaUpdaterAPI.Models;

public class Source
{
    public Source(int id, string name, string baseURL)
    {
        Id = id;
        Name = name;
        BaseURL = baseURL;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string BaseURL { get; set; }
}
