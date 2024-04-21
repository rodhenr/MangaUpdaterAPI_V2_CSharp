using System.Net.Http.Json;
using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Models;
using MediatR;

namespace MangaUpdater.Core.Features.External;

public record GetMangaInfoFromMyAnimeListQuery(int MalMangaId) : IRequest<GetMangaInfoFromMyAnimeListResponse>;
public record GetMangaInfoFromMyAnimeListResponse(string Synopsis, string Type, string CoverUrl, IEnumerable<TitleEntry> Titles, IEnumerable<MalCollection> Genres, IEnumerable<MalCollection> Authors);

public sealed class GetMangaInfoFromMyAnimeListHandler : IRequestHandler<GetMangaInfoFromMyAnimeListQuery, GetMangaInfoFromMyAnimeListResponse>
{
    private readonly IHttpClientFactory _clientFactory;
    
    public GetMangaInfoFromMyAnimeListHandler(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<GetMangaInfoFromMyAnimeListResponse> Handle(GetMangaInfoFromMyAnimeListQuery request, CancellationToken cancellationToken)
    {
        var client = _clientFactory.CreateClient();
        var url = $"https://api.jikan.moe/v4/manga/{request.MalMangaId}";

        var response = await client.GetAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode) throw new BadRequestException($"Invalid ID {request.MalMangaId} from MyAnimeList");

        var content = await response.Content.ReadFromJsonAsync<MyAnimeListApiModel>(cancellationToken);

        var apiResponse = content?.Data;
        
        if (apiResponse is null) throw new BadRequestException($"Invalid response from MyAnimeList for ID {request.MalMangaId}");
        
        return new GetMangaInfoFromMyAnimeListResponse(apiResponse.Synopsis, apiResponse.Type, apiResponse.Images.JPG.LargeImageUrl, apiResponse.Titles, apiResponse.Genres, apiResponse.Authors);
    }
}