using MangaUpdater.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.Sources.Queries;

public record GetSourcesQuery : IRequest<List<GetSourcesResponse>>;

public record GetSourcesResponse(int Id, string Name, string Url);

public sealed class GetSourcesHandler : IRequestHandler<GetSourcesQuery, List<GetSourcesResponse>>
{
    private readonly AppDbContextIdentity _context;

    public GetSourcesHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<List<GetSourcesResponse>> Handle(GetSourcesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Sources
            .Select(x => new GetSourcesResponse(x.Id, x.Name, x.BaseUrl))
            .ToListAsync(cancellationToken);
    }
}