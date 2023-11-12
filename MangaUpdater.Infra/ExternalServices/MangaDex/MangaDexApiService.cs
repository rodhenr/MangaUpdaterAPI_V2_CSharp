using System.Globalization;
using MangaUpdater.Application.Interfaces.External.MangaDex;
using MangaUpdater.Application.Models.External.MangaDex;
using MangaUpdater.Domain.Exceptions;
using MangaUpdater.Infra.Data.Extensions;

namespace MangaUpdater.Infra.Data.ExternalServices.MangaDex;

public class MangaDexApiService : IMangaDexApi
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly List<MangaDexResponse> _list = new();

    public MangaDexApiService(IHttpClientFactory httpClientFactory)
    {
        _clientFactory = httpClientFactory;
    }

    public async Task<List<MangaDexResponse>> GetChaptersAsync(string mangaDexId, float initialChapter)
    {
        var httpClient = _clientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "MangaUpdater/1.0");

        var offset = 0;

        while (true)
        {
            var url =
                $"https://api.mangadex.org/manga/{mangaDexId}/feed?translatedLanguage[]=en&limit=199&order[chapter]=asc&offset={offset}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new BadRequestException(
                    $"Failed to retrieve data for ID `{mangaDexId}` from MangaDex");

            var content = await response.Content.TryToReadJsonAsync<MangaDexApiData>();

            if (content?.Data.Count == 0) break;

            foreach (var chapter in content.Data)
            {
                if (float.Parse(chapter.Attributes.Chapter, CultureInfo.InvariantCulture) > initialChapter)
                {
                    _list.Add(chapter);
                }
            }

            offset += 200;
        }

        return _list;
    }
}