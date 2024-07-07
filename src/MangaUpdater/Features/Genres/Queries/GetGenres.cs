using MangaUpdater.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.Genres.Queries;

public record GetGenresQuery : IRequest<List<GetGenresResponse>>;

public record GetGenresResponse(int Id, string Name);

public sealed class GetGenresHandler : IRequestHandler<GetGenresQuery, List<GetGenresResponse>>
{
    private readonly AppDbContextIdentity _context;
    
    public GetGenresHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<List<GetGenresResponse>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
    {
        return await _context.MangaGenres
            .Select(x => new GetGenresResponse(x.Genre.Id, x.Genre.Name))
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}