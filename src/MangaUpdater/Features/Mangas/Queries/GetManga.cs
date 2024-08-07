﻿using System.Globalization;
using MangaUpdater.Database;
using MangaUpdater.DTOs;
using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Helpers;
using MangaUpdater.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MangaUpdater.Mappers;

namespace MangaUpdater.Features.Mangas.Queries;

public record GetMangaQuery([FromRoute] int MangaId) : IRequest<GetMangaResponse>;

public record GetMangaResponse(
    int Id, 
    string CoverUrl, 
    string Synopsis, 
    string Type, 
    bool IsUserFollowing, 
    IEnumerable<ChapterDto> Chapters, 
    IEnumerable<GenreDto> Genres, 
    IEnumerable<SourceDto> Sources, 
    IEnumerable<MangaAuthorDto> Authors, 
    IEnumerable<MangaTitleDto> Titles
);

public sealed class GetMangaHandler : IRequestHandler<GetMangaQuery, GetMangaResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public GetMangaHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<GetMangaResponse> Handle(GetMangaQuery request, CancellationToken cancellationToken)
    {
        var queryable = _context.Mangas.AsNoTracking();
        var manga = await ApplyFilters(queryable, request.MangaId, cancellationToken) 
                    ?? throw new EntityNotFoundException($"Manga not found for ID {request.MangaId}");
        
        manga.Chapters = manga.Chapters
            .OrderByDescending(ch => float.Parse(ch.Number, CultureInfo.InvariantCulture))
            .ToList();
        
        var isUserFollowing = manga.UserMangas.Count > 0;
        
        var chapters = MapAndReturnChapters(manga);
        
        return new GetMangaResponse(
            manga.MyAnimeListId, 
            manga.CoverUrl, 
            manga.Synopsis, 
            manga.Type, 
            isUserFollowing, 
            chapters, 
            manga.MangaGenres.ToDtos(), 
            manga.MangaSources.ToDtos(), 
            manga.MangaAuthors.ToDtos(), 
            manga.MangaTitles.ToDtos()
        );
    }

    private async Task<Manga?> ApplyFilters(IQueryable<Manga> queryable, int mangaId, CancellationToken cancellationToken)
    {
        var userId = _currentUserAccessor.IsLoggedIn ? _currentUserAccessor.UserId : null;
        
        queryable = queryable
            .Include(m => m.MangaGenres)
            .ThenInclude(mg => mg.Genre)
            .Include(m => m.MangaSources)
            .ThenInclude(ms => ms.Source)
            .Include(m => m.Chapters)
            .ThenInclude(ms => ms.Source)
            .Include(m => m.MangaAuthors)
            .Include(m => m.MangaTitles);

        if (userId is not null)
        {
            queryable = queryable
                .Include(m => m.UserMangas.Where(um => um.UserId == userId))
                .ThenInclude(um => um.UserChapters)
                .ThenInclude(uc => uc.Chapter);
        }

        return await queryable.SingleOrDefaultAsync(m => m.MyAnimeListId == mangaId, cancellationToken);
    }

    private IEnumerable<ChapterDto> MapAndReturnChapters(Manga manga)
    {
        if (manga.Chapters.Count == 0) return Enumerable.Empty<ChapterDto>();

        if (!_currentUserAccessor.IsLoggedIn) return manga.Chapters.ToDtos();

        var userMangaInfo = manga.UserMangas
            .SelectMany(um => um.UserChapters, (um, uc) => new UserMangaDto(
                um.MangaId, 
                uc.SourceId, 
                uc.ChapterId, 
                uc.Chapter?.Number
            ))
            .ToList();

        return UserMangaChapterInfo.GetUserChaptersState(manga.Chapters, userMangaInfo);
    }
}