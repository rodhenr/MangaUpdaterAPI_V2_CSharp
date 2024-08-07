﻿using MangaUpdater.Database;
using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.Mangas.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.Mangas.Commands;

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

        if (isMangaRegistered is not null)
        {
            throw new BadRequestException($"The ID {request.MalId} is already registered");
        }
        
        var apiResponse = await _mediator.Send(new GetMangaInfoFromMyAnimeListQuery(request.MalId), cancellationToken);

        await CreateManga(request.MalId, apiResponse, cancellationToken);
        await CreateMangaDetails(apiResponse, request.MalId, cancellationToken);
    }

    private async Task CreateManga(int malId, GetMangaInfoFromMyAnimeListResponse apiResponse, CancellationToken cancellationToken)
    {
        var manga = new Manga
        {
            MyAnimeListId = malId,
            Synopsis = apiResponse.Synopsis,
            Type = apiResponse.Type,
            CoverUrl = apiResponse.CoverUrl
        };
        
        _context.Mangas.Add(manga);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task CreateMangaDetails(GetMangaInfoFromMyAnimeListResponse apiResponse, int mangaId, CancellationToken cancellationToken)
    {
        await CreateMissingGenres(apiResponse, cancellationToken);
        
        var genreList = apiResponse.Genres.Select(g => new MangaGenre
        {
            GenreId = g.MalId, 
            MangaId = mangaId 
        });
        
        var authorList = apiResponse.Authors.Select(a => new MangaAuthor
        {
            MangaId = mangaId, 
            Name = a.Name.Replace(',', ' ')
        });
        
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

    private async Task CreateMissingGenres(GetMangaInfoFromMyAnimeListResponse apiResponse, CancellationToken cancellationToken)
    {
        var genreIds = await _context.Genres.Select(x => x.Id).ToListAsync(cancellationToken);

        var genresToCreate = apiResponse.Genres
            .Where(x => !genreIds.Contains(x.MalId))
            .Select(x => new Genre
            {
                Id = x.MalId,
                Name = x.Name
            })
            .ToList();

        if (genresToCreate.Count > 0)
        {
            _context.Genres.AddRange(genresToCreate);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}