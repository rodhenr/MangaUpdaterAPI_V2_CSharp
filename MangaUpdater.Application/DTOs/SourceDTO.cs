namespace MangaUpdater.Application.DTOs;

public class SourceDTO
{
    public SourceDTO(string name, string baseURL)
    {
        Name = name;
        BaseURL = baseURL;
    }

    public string Name { get; set; }
    public string BaseURL { get; set; }
}
