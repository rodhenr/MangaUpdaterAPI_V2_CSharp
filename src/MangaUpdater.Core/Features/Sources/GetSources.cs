using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Sources;

public record GetSourcesQuery : IRequest<GetSourcesResponse>;
public record GetSourcesResponse(List<Source> Sources);

public sealed class GetSourcesHandler : IRequestHandler<GetSourcesQuery, GetSourcesResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetSourcesHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetSourcesResponse> Handle(GetSourcesQuery request, CancellationToken cancellationToken)
    {
        var sources = await _context.Sources
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new GetSourcesResponse(sources);
    }
}