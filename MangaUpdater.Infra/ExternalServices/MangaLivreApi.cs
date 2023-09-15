using System.Net.Http.Json;
using MangaUpdater.Application.Models;

namespace MangaUpdater.Infra.Data.ExternalServices;

public class MangaLivreApi
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly List<MangaLivreChapters> _chapters = new();

    public MangaLivreApi(IHttpClientFactory httpClientFactory)
    {
        _clientFactory = httpClientFactory;
    }

    public async Task<List<MangaLivreChapters>> GetChaptersAsync(int mlSerieId, float lastChapterId = 0)
    {
        var httpClient = _clientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        var page = 1;

        while (true)
        {
            var baseUrl = $"https://mangalivre.net/series/chapters_list.json?page={page}&id_serie={mlSerieId}";
            var response = await httpClient.GetAsync($"{baseUrl}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Invalid serie id {mlSerieId}");

            var content = await response.Content.TryToReadJsonAsync<MangaLivreChaptersData>();

            if (content?.Chapters is null) break;

            SaveChapters(content.Chapters, lastChapterId);

            page += 1;
        }

        return _chapters;
    }

    private void SaveChapters(List<MangaLivreChapters> chapters, float lastChapterId)
    {
        if (lastChapterId == 0)
        {
            _chapters.AddRange(chapters);
            return;
        }

        foreach (var chapter in chapters)
        {
            if (float.Parse(chapter.ChapterNumber) <= lastChapterId) return;
            _chapters.Add(chapter);
        }
    }
}

public static class ExtensionJson
{
    public static async Task<T?> TryToReadJsonAsync<T>(this HttpContent httpContent) where T : class =>
        await Handle<T>(httpContent);

    private static async Task<T?> Handle<T>(HttpContent httpContent) where T : class
    {
        try
        {
            var jsonRead = await httpContent.ReadFromJsonAsync<T>();
            return jsonRead;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}