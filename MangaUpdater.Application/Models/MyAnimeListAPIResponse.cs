using System.Text.Json.Serialization;

namespace MangaUpdater.Application.Models;

public class MyAnimeListAPIResponse
{
    public MyAnimeListAPIResponse(long malId, IEnumerable<TitleEntry> titles, ImagesSet images, string type, string status, bool publishing, string synopsis, IEnumerable<MalCollection> genres, IEnumerable<MalCollection> authors, IEnumerable<MalCollection> serializations)
    {
        MalId = malId;
        Titles = titles;
        Images = images;
        Type = type;
        Status = status;
        Publishing = publishing;
        Synopsis = synopsis;
        Genres = genres;
        Authors = authors;
        Serializations = serializations;
    }

    [JsonPropertyName("mal_id")]
    public long MalId { get; set; }

    [JsonPropertyName("titles")]
    public IEnumerable<TitleEntry> Titles { get; set; }

    [JsonPropertyName("images")]
    public ImagesSet Images { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("publishing")]
    public bool Publishing { get; set; }

    [JsonPropertyName("synopsis")]
    public string Synopsis { get; set; }

    [JsonPropertyName("genres")]
    public IEnumerable<MalCollection> Genres { get; set; }

    [JsonPropertyName("authors")]
    public IEnumerable<MalCollection> Authors { get; set; }

    [JsonPropertyName("serializations")]
    public IEnumerable<MalCollection> Serializations { get; set; }
}

public class MalCollection
{
    [JsonPropertyName("mal_id")]
    public long MalId { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class ImagesSet
{
    [JsonPropertyName("jpg")]
    public Image JPG { get; set; }

    [JsonPropertyName("webp")]
    public Image WebP { get; set; }
}

public class Image
{
    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; }

    [JsonPropertyName("small_image_url")]
    public string SmallImageUrl { get; set; }

    [JsonPropertyName("medium_image_url")]
    public string MediumImageUrl { get; set; }

    [JsonPropertyName("large_image_url")]
    public string LargeImageUrl { get; set; }

    [JsonPropertyName("maximum_image_url")]
    public string MaximumImageUrl { get; set; }
}

public class TitleEntry
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }
}