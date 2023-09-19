using System.Text.Json.Serialization;
using MangaUpdater.Application.Models.External.MyAnimeList;

namespace MangaUpdater.Infra.Data.ExternalServices.MyAnimeList;

public class MyAnimeListApiData
{
    [JsonPropertyName("data")] public required MyAnimeListApiResponse Data { get; set; }
}