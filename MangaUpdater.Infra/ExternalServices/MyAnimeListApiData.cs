using System.Text.Json.Serialization;
using MangaUpdater.Application.Models;

namespace MangaUpdater.Infra.Data.ExternalServices;

public class MyAnimeListApiData
{
    [JsonPropertyName("data")] public MyAnimeListApiResponse? Data { get; set; }
}