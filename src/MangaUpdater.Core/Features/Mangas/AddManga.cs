using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Features.External;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Mangas;

public record AddMangaCommand([FromBody] int MalId) : IRequest;

public sealed class AddMangaHandler : IRequestHandler<AddMangaCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly IMediator _mediator;
    
    public AddMangaHandler(AppDbContextIdentity context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(AddMangaCommand request, CancellationToken cancellationToken)
    {
        var isMangaRegistered = await _context.Mangas
            .Where(x => x.MyAnimeListId == request.MalId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (isMangaRegistered is not null) throw new BadRequestException($"The ID {request.MalId} is already registered");
        
        var apiResponse = await _mediator.Send(new GetMangaInfoFromMyAnimeListQuery(request.MalId), cancellationToken);

        var createdMangaId = await CreateManga(request.MalId, apiResponse, cancellationToken);
        await CreateMangaDetails(apiResponse, createdMangaId, cancellationToken);
    }

    private async Task<int> CreateManga(int malId, GetMangaInfoFromMyAnimeListResponse apiResponse, CancellationToken cancellationToken)
    {
        var manga = new Manga
        {
            Synopsis = apiResponse.Synopsis,
            Type = apiResponse.Type,
            CoverUrl = apiResponse.CoverUrl,
            MyAnimeListId = malId
        };
        
        _context.Mangas.Add(manga);
        await _context.SaveChangesAsync(cancellationToken);

        return malId;
    }

    private async Task CreateMangaDetails(GetMangaInfoFromMyAnimeListResponse apiResponse, int mangaId, CancellationToken cancellationToken)
    {
        var genreList = apiResponse.Genres.Select(g => new MangaGenre { GenreId = (int)g.MalId, MangaId = mangaId });
        var authorList = apiResponse.Authors.Select(a => new MangaAuthor { MangaId = mangaId, Name = a.Name });
        var titleList = apiResponse.Titles
            .Select(i => i.Title)
            .Distinct()
            .Select((title, index) =>
                new MangaTitle
                {
                    MangaId = mangaId,
                    Name = title,
                    IsMyAnimeListMainTitle = index == 0
                });

        _context.MangaGenres.AddRange(genreList);
        _context.MangaAuthors.AddRange(authorList);
        _context.MangaTitles.AddRange(titleList);

        await _context.SaveChangesAsync(cancellationToken);
    }
}