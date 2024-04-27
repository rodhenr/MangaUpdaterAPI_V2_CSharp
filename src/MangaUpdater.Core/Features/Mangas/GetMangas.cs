using MangaUpdater.Core.Dto;
using MangaUpdater.Core.Features.Chapters;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Mangas;

public enum OrderByEnum
{
    Alphabet,
    Latest
}

public record GetMangasQuery(string? Input, int Page = 1, int PageSize = 20, OrderByEnum OrderBy = OrderByEnum.Alphabet, List<int>? SourceIds = null, List<int>? GenreIds = null) : IRequest<GetMangasResponse>;

public record GetMangasResponse(int CurrentPage, int PageSize, int TotalPages, List<MangaInfo> Mangas);

public record MangaInfo(int MangaId, string CoverUrl, string MangaName, List<ChapterDto> RecentChapters);

public sealed class GetMangasHandler : IRequestHandler<GetMangasQuery, GetMangasResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly IMediator _mediator;
    
    public GetMangasHandler(AppDbContextIdentity context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<GetMangasResponse> Handle(GetMangasQuery request, CancellationToken cancellationToken)
    {
        var queryable = _context.Mangas
            .Include(m => m.MangaTitles)
            .Include(m => m.MangaSources)
            .Include(m => m.MangaGenres)
            .AsQueryable();

        queryable = ApplyFilters(request, queryable);
        
        var mangas = await queryable
            .AsNoTracking()
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var mangasDto = await GetMangaInfo(mangas, cancellationToken);
        var numberOfPages = await GetNumberOfPages(request.PageSize, cancellationToken);

        return new GetMangasResponse(request.Page, request.PageSize, numberOfPages, mangasDto);
    }

    private static IQueryable<Manga> ApplyFilters(GetMangasQuery request, IQueryable<Manga> queryable)
    {
        queryable = request.OrderBy switch
        {
            OrderByEnum.Alphabet => queryable.OrderBy(m => m.MangaTitles.First(mt => mt.IsMyAnimeListMainTitle).Name),
            OrderByEnum.Latest => queryable.OrderByDescending(m => m.Id),
            _ => queryable
        };

        if (request.SourceIds is not null && request.SourceIds.Count != 0) queryable = queryable.Where(m => m.MangaSources.Any(b => request.SourceIds.Contains(b.SourceId)));
        if (request.GenreIds is not null && request.GenreIds.Count != 0) queryable = queryable.Where(m => m.MangaGenres.Any(b => request.GenreIds.Contains(b.GenreId)));
        
        if (!string.IsNullOrWhiteSpace(request.Input)) queryable = queryable.Where(m => m.MangaTitles.Any(mt => mt.Name.Contains(request.Input)));

        return queryable;
    }

    private async Task<List<MangaInfo>> GetMangaInfo(List<Manga> mangas, CancellationToken cancellationToken)
    {
        var mangaInfo = new List<MangaInfo>();
        
        foreach (var manga in mangas)
        {
            var recentChapters = await _mediator.Send(new GetRecentChaptersQuery(manga.Id, manga.MangaSources.Select(x => x.SourceId).ToList()), cancellationToken);
            mangaInfo.Add(new MangaInfo(manga.Id, manga.CoverUrl, manga.MangaTitles.First(mt => mt.IsMyAnimeListMainTitle).Name, recentChapters.Chapters.ToList()));
        }

        return mangaInfo;
    }

    private async Task<int> GetNumberOfPages(int pageSize, CancellationToken cancellationToken)
    {
        var numberOfMangas = await _context.Mangas
            .AsNoTracking()
            .CountAsync(cancellationToken);

        return numberOfMangas / pageSize + 1;
    }
}