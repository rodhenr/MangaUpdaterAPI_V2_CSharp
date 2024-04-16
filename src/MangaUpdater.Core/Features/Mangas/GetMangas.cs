using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Data;

namespace MangaUpdater.Core.Features.Mangas;

public record GetMangasQuery([FromQuery] int Page = 1, [FromQuery] int PageSize = 20, [FromQuery] string? OrderBy = null, [FromQuery] List<int>? SourceIds = null, [FromQuery] List<int>? GenreIds = null, [FromQuery] string? Input = null) : IRequest<GetMangasResponse>;
public record GetMangasResponse(int CurrentPage, int PageSize, int TotalPages, List<MangaInfo> Mangas);
public record MangaInfo(int MangaId, string CoverUrl, string MangaName);

public sealed class GetMangasHandler : IRequestHandler<GetMangasQuery, GetMangasResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetMangasHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetMangasResponse> Handle(GetMangasQuery request, CancellationToken cancellationToken)
    {
        // I WAS RETURNING GENRES HERE, I THINK I SHOULD NOT RETURN IT ANYMORE
        // Filter
        var query = _context.Mangas
            .Include(m => m.MangaTitles)
            .Include(m => m.MangaSources)
            .Include(m => m.MangaGenres)
            .AsQueryable();

        query = request.OrderBy switch
        {
            "alphabet" => query.OrderBy(m => m.MangaTitles!.First(mt => mt.IsMainTitle).Name),
            "latest" => query.OrderByDescending(m => m.Id),
            _ => query
        };

        if (request.SourceIds?.Count != 0)
        {
            query = query
                .Where(m => m.MangaSources != null && m.MangaSources.Any(b => request.SourceIds!.Contains(b.SourceId)))
                .Include(m => m.MangaSources);
        }

        if (request.GenreIds?.Count != 0)
        {
            query = query
                .Where(m => m.MangaGenres != null && m.MangaGenres.Any(b => request.GenreIds!.Contains(b.GenreId)))
                .Include(m => m.MangaGenres);
        }

        if (request.Input is not null)
        {
            query = query.Where(m => m.MangaTitles!.Any(mt => mt.Name.Contains(request.Input)));
        }
        
        //  Result
        var mangas = await query
            .AsNoTracking()
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(manga => new MangaInfo(manga.Id, manga.CoverUrl, manga.MangaTitles!.First(mt => mt.IsMainTitle).Name))
            .ToListAsync(cancellationToken);

        var numberOfMangas = await query
            .AsNoTracking()
            .CountAsync(cancellationToken);

        var numberOfPages = numberOfMangas / request.PageSize;

        return new GetMangasResponse(0, 0, numberOfPages, mangas);
    }
}