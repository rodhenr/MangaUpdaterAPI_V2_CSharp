using MangaUpdater.Application.Models;
using System.Text.Json.Serialization;

namespace MangaUpdater.Infra.Data.ExternalServices;

public class MyAnimeListAPIData
{
    [JsonPropertyName("data")]
    public MyAnimeListAPIResponse Data { get; set; }
}
