using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace MangaUpdater.Infra.Data.ExternalServices;

public class MyAnimeListAPIService : IMyAnimeListAPIService
{
    private readonly IHttpClientFactory _clientFactory;

    public MyAnimeListAPIService(IHttpClientFactory httpClientFactory)
    {
        _clientFactory = httpClientFactory;
    }

    public async Task<MyAnimeListAPIResponse?> GetMangaByIdAsync(int malMangaId)
    {
        var client = _clientFactory.CreateClient();
        var url = $"https://api.jikan.moe/v4/manga/{malMangaId}";

        HttpResponseMessage response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }


        MyAnimeListAPIData? content = await response.Content.ReadFromJsonAsync<MyAnimeListAPIData>();

        return content?.Data;
    }
}
