using System.Globalization;
using MangaUpdater.Application.Interfaces.External.MangaDex;
using MangaUpdater.Application.Models.External.MangaDex;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Exceptions;
using MangaUpdater.Infra.Data.Extensions;

namespace MangaUpdater.Infra.Data.ExternalServices.MangaDex;

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

            if (content?.Data.Count == 0) break;

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