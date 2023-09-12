using System.Text.Json.Serialization;

namespace MangaUpdater.Application.Models;

public record MyAnimeListApiResponse
{
    [JsonPropertyName("mal_id")] public required long MalId { get; set; }

    [JsonPropertyName("titles")] public required IEnumerable<TitleEntry> Titles { get; set; }

    [JsonPropertyName("images")] public required ImagesSet Images { get; set; }

    [JsonPropertyName("type")] public required string Type { get; set; }

    [JsonPropertyName("status")] public required string Status { get; set; }

    [JsonPropertyName("publishing")] public required bool Publishing { get; set; }

    [JsonPropertyName("synopsis")] public required string Synopsis { get; set; }

    [JsonPropertyName("genres")] public required IEnumerable<MalCollection> Genres { get; set; }

    [JsonPropertyName("authors")] public required IEnumerable<MalCollection> Authors { get; set; }
}

public class MalCollection
{
    [JsonPropertyName("mal_id")] public required long MalId { get; set; }

    [JsonPropertyName("type")] public required string Type { get; set; }

    [JsonPropertyName("url")] public required string Url { get; set; }

    [JsonPropertyName("name")] public required string Name { get; set; }
}

public class ImagesSet
{
    [JsonPropertyName("jpg")] public required Image JPG { get; set; }
}

public class Image
{
    [JsonPropertyName("large_image_url")] public required string LargeImageUrl { get; set; }
}

public class TitleEntry
{
    [JsonPropertyName("type")] public required string Type { get; set; }

    [JsonPropertyName("title")] public required string Title { get; set; }
}