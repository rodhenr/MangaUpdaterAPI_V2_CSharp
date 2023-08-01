namespace MangaUpdater.Application.DTOs;

public class SourceDTO
{
    public SourceDTO(int id, string name, string baseURL)
    {
        Id = id;
        Name = name;
        BaseURL = baseURL;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string BaseURL { get; set; }
}
