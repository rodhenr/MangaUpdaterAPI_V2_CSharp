using System.Globalization;
using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Dto;
using MangaUpdater.Core.Services;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Mangas;

public record GetMangaQuery([FromRoute] int MangaId) : IRequest<GetMangaResponse>;

public record GetMangaResponse(int Id, string CoverUrl, string Synopsis, string Type, int MyAnimeListId, IEnumerable<ChapterDto> Chapters, IEnumerable<GenreDto> Genres, IEnumerable<SourceDto> Sources, IEnumerable<AuthorDto> Authors, IEnumerable<TitleDto> Titles);

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
        var result = await ApplyFilters(queryable, request.MangaId, cancellationToken);

        if (result is null) throw new EntityNotFoundException($"Manga not found for ID {request.MangaId}");
        
        result.Chapters = result.Chapters
            .OrderByDescending(ch => float.Parse(ch.Number, CultureInfo.InvariantCulture))
            .ToList();

        var chapters = MapAndReturnChapters(result);
        var genres = result.MangaGenres.Select(x => new GenreDto(x.Genre.Id, x.Genre.Name));
        var sources = result.MangaSources.Select(x => new SourceDto(x.Source.Id, x.Source.Name));
        var authors = result.MangaAuthors.Select(x => new AuthorDto(x.Id, x.Name));
        var titles = result.MangaTitles.Select(x => new TitleDto(x.Id, x.Name, x.IsMainTitle));
        
        return new GetMangaResponse(result.Id, result.CoverUrl, result.Synopsis, result.Type, result.MyAnimeListId, chapters, genres, sources, authors, titles);
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

        return await queryable.SingleOrDefaultAsync(m => m.Id == mangaId, cancellationToken);
    }

    private IEnumerable<ChapterDto> MapAndReturnChapters(Manga manga)
    {
        if (manga.Chapters.Count == 0) return Enumerable.Empty<ChapterDto>();
        
        var isAuthenticated = _currentUserAccessor.IsLoggedIn;
        if (!isAuthenticated) return manga.Chapters.Select(x => new ChapterDto(x.Id, x.SourceId, x.Source.Name, x.Date, x.Number));

        var userMangaInfo = manga.UserMangas
            .SelectMany(um => um.UserChapters, (um, uc) => new { um.MangaId, uc.SourceId, uc.ChapterId, uc.Chapter?.Number })
            .ToList();

        return manga.Chapters
            .Select(ch =>
            {
                var chapterInfo = userMangaInfo.FirstOrDefault(um => um.SourceId == ch.SourceId && um.MangaId == ch.MangaId);
                var isRead = float.Parse(ch.Number, CultureInfo.InvariantCulture) <= float.Parse(chapterInfo?.Number ?? "0", CultureInfo.InvariantCulture);

                return new ChapterDto(ch.Id, ch.Source!.Id, ch.Source.Name, ch.Date, ch.Number, chapterInfo is not null, chapterInfo is not null && isRead);
            });
    }
}