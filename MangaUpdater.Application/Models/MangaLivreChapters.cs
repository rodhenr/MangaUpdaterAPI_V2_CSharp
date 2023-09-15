using System.Text.Json.Serialization;

namespace MangaUpdater.Application.Models;

public record MangaLivreChapters
{
    [JsonPropertyName("chapter_name")] public required string ChapterName { get; set; }
    [JsonPropertyName("number")] public required string ChapterNumber { get; set; }
    [JsonPropertyName("date")] public required string ChapterDate { get; set; }
    [JsonPropertyName("releases")] public required Dictionary<string, MangaLivreReleaseInfo> ReleaseList { get; set; }
}