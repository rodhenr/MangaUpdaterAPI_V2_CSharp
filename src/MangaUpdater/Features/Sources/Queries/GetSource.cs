using MangaUpdater.Database;
using MangaUpdater.Exceptions;
using MangaUpdater.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.Features.Sources.Queries;

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
        var source = await _context.Sources.GetById(request.SourceId, cancellationToken) 
                     ?? throw new EntityNotFoundException($"Source not found for SourceId {request.SourceId}.");
        
        return new GetSourceResponse(source.Id, source.Name, source.BaseUrl);
    }
}