namespace MangaUpdater.Application.DTOs;

public record MangaUserDTO
{
    public MangaUserDTO(int id, string coverURL, string name)
    {
        Id = id;
        CoverURL = coverURL;
        Name = name;
    }

    public int Id { get; set; }

    public string CoverURL { get; set; }

    public string Name { get; set; }
}
