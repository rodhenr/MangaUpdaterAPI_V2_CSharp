using System.Text.Json.Serialization;
using MangaUpdater.Application.Models.External.MangaLivre;

namespace MangaUpdater.Infra.Data.ExternalServices.MangaLivre;

public class MangaLivreChaptersData
{
    [JsonPropertyName("chapters")] public required List<MangaLivreChapters> Chapters { get; set; }
}