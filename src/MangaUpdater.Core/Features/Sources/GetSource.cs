using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Sources;

public record GetSourceQuery([FromQuery] int SourceId) : IRequest<GetSourceResponse>;
public record GetSourceResponse(Source? Source);

public sealed class GetSourceHandler : IRequestHandler<GetSourceQuery, GetSourceResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetSourceHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetSourceResponse> Handle(GetSourceQuery request, CancellationToken cancellationToken)
    {
        var source = await _context.Sources
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.SourceId, cancellationToken);

        return new GetSourceResponse(source);
    }
}