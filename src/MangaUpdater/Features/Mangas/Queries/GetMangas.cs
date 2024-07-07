using MangaUpdater.Infrastructure;
using MangaUpdater.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.Mangas.GetMangas;

public enum OrderByEnum
{
    Alphabet,
    Latest
}

public record GetMangasQuery(
    string? Input, 
    int Page = 1, 
    int PageSize = 20, 
    OrderByEnum OrderBy = OrderByEnum.Alphabet, 
    List<int>? SourceIds = null, 
    List<int>? GenreIds = null
) : IRequest<GetMangasResponse>;

public record GetMangasResponse(int CurrentPage, int PageSize, int TotalPages, IEnumerable<MangaInfo> Mangas);

public record MangaInfo(int MangaId, string CoverUrl, string MangaName);

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
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize < 1 ? 1 : request.PageSize;
        
        var queryable = _context.Mangas.AsNoTracking();
        queryable = ApplyFilters(request, queryable);
        
        var mangas = await queryable
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new MangaInfo(x.MyAnimeListId, x.CoverUrl, x.MangaTitles.First().Name))
            .ToListAsync(cancellationToken);
        
        var count = await queryable.CountAsync(cancellationToken);
        var numberOfPages = (int)Math.Ceiling((double)count / pageSize);

        return new GetMangasResponse(request.Page, request.PageSize, numberOfPages, mangas);
    }

    private static IQueryable<Manga> ApplyFilters(GetMangasQuery request, IQueryable<Manga> queryable)
    {
        queryable = request.OrderBy switch
        {
            OrderByEnum.Alphabet => queryable.OrderBy(m => m.MangaTitles.First(mt => mt.IsMyAnimeListMainTitle).Name),
            OrderByEnum.Latest => queryable.OrderByDescending(m => m.MyAnimeListId),
            _ => queryable
        };

        if (request.SourceIds is not null && request.SourceIds.Count != 0)
        {
            queryable = queryable.Where(m => m.MangaSources.Any(b => request.SourceIds.Contains(b.SourceId)));
        }

        if (request.GenreIds is not null && request.GenreIds.Count != 0)
        {
            queryable = queryable.Where(m => m.MangaGenres.Any(b => request.GenreIds.Contains(b.GenreId)));
        }
        
        if (!string.IsNullOrWhiteSpace(request.Input)) queryable = queryable.Where(m => m.MangaTitles.Any(mt => mt.Name.Contains(request.Input)));

        return queryable;
    }
}