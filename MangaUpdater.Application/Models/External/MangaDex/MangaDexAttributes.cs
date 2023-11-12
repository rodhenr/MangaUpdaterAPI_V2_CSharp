namespace MangaUpdater.Application.Models.External.MangaDex;

public class MangaDexAttributes
{
    public string? Volume { get; set; }
    public required string Chapter { get; set; }
    public string? Title { get; set; }
    public required string TranslatedLanguage { get; set; }
    public string? ExternalUrl  { get; set; }
    public required string PublishAt { get; set; }
    public required string ReadableAt { get; set; }
    public required string CreatedAt { get; set; }
    public required string UpdatedAt { get; set; }
    public int Pages { get; set; }
    public int Version { get; set; }
}