using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Data;
using MangaUpdater.Data.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.Core.Features.Sources;

public record GetSourceQuery([FromRoute] int SourceId) : IRequest<GetSourceResponse>;
public record GetSourceResponse(int Id, string Name, string Url);

public sealed class GetSourceHandler : IRequestHandler<GetSourceQuery, GetSourceResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetSourceHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetSourceResponse> Handle(GetSourceQuery request, CancellationToken cancellationToken)
    {
        var source = await _context.Sources.GetById(request.SourceId, cancellationToken);

        if (source is null) throw new EntityNotFoundException($"Source not found for SourceId {request.SourceId}.");
        
        return new GetSourceResponse(source.Id, source.Name, source.BaseUrl);
    }
}