using MangaUpdater.Application.Interfaces.External.MangaLivre;
using MangaUpdater.Application.Models.External.MangaLivre;
using MangaUpdater.Domain.Exceptions;
using MangaUpdater.Infra.Data.Extensions;

namespace MangaUpdater.Infra.Data.ExternalServices.MangaLivre;

public class MangaLivreApiService : IMangaLivreApi
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly List<MangaLivreChapters> _apiChapters = new();

    public MangaLivreApiService(IHttpClientFactory httpClientFactory)
    {
        _clientFactory = httpClientFactory;
    }

    public async Task<List<MangaLivreChapters>> GetChaptersAsync(int mangaLivreSerieId, float lastSavedChapterId = 0)
    {
        var httpClient = _clientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        var page = 1;

        while (true)
        {
            var baseUrl = $"https://mangalivre.net/series/chapters_list.json?page={page}&id_serie={mangaLivreSerieId}";
            var response = await httpClient.GetAsync($"{baseUrl}");

            if (!response.IsSuccessStatusCode)
                throw new BadRequestException(
                    $"Failed to retrieve data for ID {mangaLivreSerieId} and page {page} from MangaLivre");

            var content = await response.Content.TryToReadJsonAsync<MangaLivreChaptersData>();

            if (content?.Chapters is null) break;

            SaveChapters(content.Chapters, lastSavedChapterId);

            page += 1;
        }

        return _apiChapters;
    }

    private void SaveChapters(IEnumerable<MangaLivreChapters> apiChapters, float lastSavedChapterId)
    {
        if (lastSavedChapterId == 0)
        {
            _apiChapters.AddRange(apiChapters);
            return;
        }

        foreach (var chapter in apiChapters.Where(chapter => float.Parse(chapter.ChapterNumber) > lastSavedChapterId))
        {
            _apiChapters.Add(chapter);
        }
    }
}