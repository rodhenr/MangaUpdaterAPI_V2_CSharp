using System.Text.Json.Serialization;

namespace MangaUpdater.Application.Models.External.MangaLivre;

public class MangaLivreScanlator
{
    [JsonPropertyName("id_scanlator")] public required int ScanlatorId { get; set; }
    [JsonPropertyName("name")] public required string Name { get; set; }
    [JsonPropertyName("link")] public required string Link { get; set; }
}