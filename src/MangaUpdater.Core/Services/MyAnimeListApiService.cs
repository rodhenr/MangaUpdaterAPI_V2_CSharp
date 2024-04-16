using System.Net.Http.Json;
using System.Text.Json.Serialization;
using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Services;

public interface IMyAnimeListApiService
{
    Task<MyAnimeListApiResponse?> GetMangaFromMyAnimeListByIdAsync(int malMangaId);
}

public class MyAnimeListApiData
{
    [JsonPropertyName("data")] public required MyAnimeListApiResponse Data { get; set; }
}


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

[RegisterScoped]
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
    
    public class RegisterMangaFromMyAnimeListService : IRegisterMangaFromMyAnimeListService
{
    private readonly IMyAnimeListApiService _malApiService;
    private readonly IMapper _mapper;
    private readonly IMangaService _mangaService;
    private readonly IMangaGenreService _mangaGenreService;
    private readonly IMangaAuthorService _mangaAuthorService;
    private readonly IMangaTitleService _mangaTitleService;

    public RegisterMangaFromMyAnimeListService(IMyAnimeListApiService malApiService, IMapper mapper,
        IMangaService mangaService,
        IMangaGenreService mangaGenreService, IMangaAuthorService mangaAuthorService,
        IMangaTitleService mangaTitleService)
    {
        _malApiService = malApiService;
        _mapper = mapper;
        _mangaService = mangaService;
        _mangaGenreService = mangaGenreService;
        _mangaAuthorService = mangaAuthorService;
        _mangaTitleService = mangaTitleService;
    }

    public async Task<Manga?> RegisterMangaFromMyAnimeListById(int malMangaId)
    {
        var isMangaRegistered = await _mangaService.CheckIfMangaIsRegistered(malMangaId);
        if (isMangaRegistered) throw new BadRequestException($"The ID {_malApiService} is already registered");

        var apiData = await _malApiService.GetMangaFromMyAnimeListByIdAsync(malMangaId);
        var manga = _mapper.Map<Manga>(apiData);
        await _mangaService.Add(manga); //TODO: Create a single transaction

        var genreList = apiData!.Genres.Select(g => new MangaGenre { GenreId = (int)g.MalId, MangaId = manga.Id });
        var authorList = apiData.Authors.Select(a => new MangaAuthor { MangaId = manga.Id, Name = a.Name });
        var titleList = apiData.Titles
            .Select(i => i.Title)
            .Distinct()
            .Select((title, index) =>
                new MangaTitle
                {
                    MangaId = manga.Id,
                    Name = title,
                    IsMainTitle = index == 0
                });

        _mangaGenreService.BulkCreate(genreList);
        _mangaAuthorService.BulkCreate(authorList);
        _mangaTitleService.BulkCreate(titleList);

        await _mangaGenreService.SaveChanges();

        return manga;
    }
}
}