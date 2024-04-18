using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Models;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.External;

public record CreateMangaFromMyAnimeListQuery(int MalMangaId) : IRequest<CreateMangaFromMyAnimeListResponse>;
public record CreateMangaFromMyAnimeListResponse;

public sealed class CreateMangaFromMyAnimeListHandler : IRequestHandler<CreateMangaFromMyAnimeListQuery, CreateMangaFromMyAnimeListResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly IHttpClientFactory _clientFactory;
    
    public CreateMangaFromMyAnimeListHandler(AppDbContextIdentity context, IHttpClientFactory clientFactory)
    {
        _context = context;
        _clientFactory = clientFactory;
    }

    public async Task<CreateMangaFromMyAnimeListResponse> Handle(CreateMangaFromMyAnimeListQuery request, CancellationToken cancellationToken)
    {
        var isMangaRegistered = await _context.Mangas
            .Where(x => x.Id == request.MalMangaId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (isMangaRegistered is not null) throw new BadRequestException($"The ID {request.MalMangaId} is already registered");
        
        var client = _clientFactory.CreateClient();
        var url = $"https://api.jikan.moe/v4/manga/{request.MalMangaId}";

        var response = await client.GetAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode) throw new BadRequestException($"Invalid ID {request.MalMangaId} from MyAnimeList");

        var content = await response.Content.ReadFromJsonAsync<MyAnimeListApiModel>(cancellationToken);

        var apiResponse = content?.Data;

        //TODO: Create a single transaction
        var createdManga = _context.Mangas.Add(apiResponse.MyAnimeListMapper(), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        var genreList = apiResponse.Genres.Select(g => new MangaGenre { GenreId = (int)g.MalId, MangaId = createdManga.Entity.Id });
        var authorList = apiResponse.Authors.Select(a => new MangaAuthor { MangaId = createdManga.Entity.Id, Name = a.Name });
        var titleList = apiResponse.Titles
            .Select(i => i.Title)
            .Distinct()
            .Select((title, index) =>
                new MangaTitle
                {
                    MangaId = createdManga.Entity.Id,
                    Name = title,
                    IsMainTitle = index == 0
                });

        _context.MangaGenres.AddRange(genreList);
        _context.MangaAuthors.AddRange(authorList);
        _context.MangaTitles.AddRange(titleList);

        await _context.SaveChangesAsync(cancellationToken);

        return new CreateMangaFromMyAnimeListResponse();
    }
}