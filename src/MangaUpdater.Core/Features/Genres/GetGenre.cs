using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Genres;

public record GetGenreQuery([FromQuery] int GenreId) : IRequest<GetGenreResponse>;
public record GetGenreResponse(Genre? Genre);

public sealed class GetGenreHandler : IRequestHandler<GetGenreQuery, GetGenreResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetGenreHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetGenreResponse> Handle(GetGenreQuery request, CancellationToken cancellationToken)
    {
        var genre = await _context.Genres
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.GenreId, cancellationToken);

        return new GetGenreResponse(genre);
    }
}