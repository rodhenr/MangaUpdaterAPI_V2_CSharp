using System.Net.Http.Json;
using MangaUpdater.Application.Interfaces.External;
using MangaUpdater.Application.Models;
using MangaUpdater.Domain.Exceptions;

namespace MangaUpdater.Infra.Data.ExternalServices;

public class MyAnimeListApiService : IMyAnimeListApiService
{
    private readonly IHttpClientFactory _clientFactory;

    public MyAnimeListApiService(IHttpClientFactory httpClientFactory)
    {
        _clientFactory = httpClientFactory;
    }

    public async Task<MyAnimeListApiResponse?> GetMangaFromMyAnimeListByIdAsync(int malMangaId)
    {
        var client = _clientFactory.CreateClient();
        var url = $"https://api.jikan.moe/v4/manga/{malMangaId}";

        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode) throw new BadRequestException($"Invalid ID {malMangaId} from MyAnimeList");

        var content = await response.Content.ReadFromJsonAsync<MyAnimeListApiData>();

        return content?.Data;
    }
}