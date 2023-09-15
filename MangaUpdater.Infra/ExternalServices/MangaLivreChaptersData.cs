using System.Text.Json.Serialization;
using MangaUpdater.Application.Models;

namespace MangaUpdater.Infra.Data.ExternalServices;

public class MangaLivreChaptersData
{
    [JsonPropertyName("chapters")] public required List<MangaLivreChapters> Chapters { get; set; }
}