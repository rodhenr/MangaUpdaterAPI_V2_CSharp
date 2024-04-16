using System.Globalization;
using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Common.Extensions;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Services;

public interface IMangaDexApi
{
    Task<List<Chapter>> GetChaptersAsync(int mangaId, int sourceId, string mangaUrl, string sourceUrl,
        string? initialChapter);
}

public class MangaDexResponse
{
    public required string Id { get; set; }
    public required string Type { get; set; }
    public MangaDexAttributes Attributes { get; set; }
    public List<MangaDexRelationships> Relationships { get; set; }
}

public class MangaDexApiData
{
    public required string Result { get; set; }
    public required string Response { get; set; }
    public List<MangaDexResponse> Data { get; set; }
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public int? Total { get; set; }
}

public class MangaDexRelationships
{
    public required string Id { get; set; }
    public required string Type { get; set; }
}

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

[RegisterScoped]
public class MangaDexApiService : IMangaDexApi
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly List<Chapter> _list = new();

    public MangaDexApiService(IHttpClientFactory httpClientFactory)
    {
        _clientFactory = httpClientFactory;
    }

    public async Task<List<Chapter>> GetChaptersAsync(int mangaId, int sourceId, string mangaUrl, string sourceUrl,
        string? initialChapter)
    {
        var httpClient = _clientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "MangaUpdater/1.0");

        var offset = 0;

        while (true)
        {
            var options = $"feed?translatedLanguage[]=en&limit=199&order[chapter]=asc&limit=500&offset={offset}";
            var url = $"{sourceUrl}{mangaUrl}/{options}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new BadRequestException(
                    $"Failed to retrieve data for ID `{mangaUrl}` from MangaDex");

            var content = await response.Content.TryToReadJsonAsync<MangaDexApiData>();

            if (content is null || content.Data.Count == 0) break;

            foreach (var chapter in content.Data)
            {
                var chapterNumber = float.Parse(chapter.Attributes.Chapter, CultureInfo.InvariantCulture);

                if (chapterNumber <= float.Parse(initialChapter ?? "0", CultureInfo.InvariantCulture)) continue;

                _list.Add(new Chapter
                {
                    MangaId = mangaId,
                    SourceId = sourceId,
                    Number = chapter.Attributes.Chapter,
                    Date = DateTime.Parse(chapter.Attributes.CreatedAt)
                });
            }

            offset += 200;
        }

        return _list;
    }
}