using System.ComponentModel.DataAnnotations;

namespace MangaUpdater.Domain.Entities;

public class Source
{
    public Source(int id, string name, string baseURL)
    {
        Id = id;
        Name = name;
        BaseURL = baseURL;
    }

    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(100)]
    public string BaseURL { get; set; }
}
