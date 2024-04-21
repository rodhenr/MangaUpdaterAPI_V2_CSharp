using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.MangaSources;

public record GetMangaSourceQuery(int MangaId, int SourceId) : IRequest<GetMangaSourceResponse>;

public record GetMangaSourceResponse(int Id, int MangaId, int SourceId, string Url);

public sealed class GetMangaSourceHandler : IRequestHandler<GetMangaSourceQuery, GetMangaSourceResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetMangaSourceHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetMangaSourceResponse> Handle(GetMangaSourceQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.MangaSources
            .AsNoTracking()
            .Where(x => x.MangaId == request.MangaId && x.SourceId == request.SourceId)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (result is null) throw new EntityNotFoundException($"MangaSource not found for MangaId {request.MangaId} and SourceId {request.SourceId}");

        return new GetMangaSourceResponse(result.Id, result.MangaId, result.SourceId, result.Url);
    }
}