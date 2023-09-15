using System.Net.Http.Json;
using MangaUpdater.Application.Models;

namespace MangaUpdater.Infra.Data.ExternalServices;

public class MangaLivreApi
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _baseUrl = "https://mangalivre.net/series/chapters_list.json?page=8&id_serie=7178";

    public MangaLivreApi(IHttpClientFactory httpClientFactory)
    {
        _clientFactory = httpClientFactory;
    }

    public async Task GetAllChaptersToRegisterSource()
    {
    }

    public async Task<List<MangaLivreChapters>> GetChaptersToUpdateSource(int mlSerieId)
    {
        var httpClient = _clientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        var page = 1;

        var result = new List<MangaLivreChapters>();

        while (true)
        {
            var baseUrl = $"https://mangalivre.net/series/chapters_list.json?page={page}&id_serie={mlSerieId}";
            var response = await httpClient.GetAsync($"{baseUrl}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Invalid serie id {mlSerieId}");

            var content = await response.Content.ReadFromJsonAsync<MangaLivreChaptersData>();

            if (content?.Chapters is null) break;

            result.AddRange(content.Chapters);

            page += 1;
        }

        return result;
    }
}