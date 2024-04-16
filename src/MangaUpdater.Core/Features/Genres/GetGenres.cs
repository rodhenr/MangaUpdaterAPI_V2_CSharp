using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Genres;

public record GetGenresQuery([FromQuery] List<int> GenreIds) : IRequest<GetGenresResponse>;
public record GetGenresResponse(List<Genre> Genres);

public sealed class GetGenresHandler : IRequestHandler<GetGenresQuery, GetGenresResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetGenresHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetGenresResponse> Handle(GetGenresQuery request, CancellationToken cancellationToken)
    {
        var genres = await _context.Genres
            .AsNoTracking()
            .Where(x => request.GenreIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return new GetGenresResponse(genres);
    }
}