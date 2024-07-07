using MangaUpdater.Database;
using MangaUpdater.Exceptions;
using MangaUpdater.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.MangaSources.Queries;

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
        var mangaSource = await _context.MangaSources
            .AsNoTracking()
            .GetMangaSourceQueryable(request.MangaId, request.SourceId, cancellationToken);

        if (mangaSource is null)
        {
            throw new EntityNotFoundException($"MangaSource not found for MangaId {request.MangaId} and SourceId {request.SourceId}");
        }

        return new GetMangaSourceResponse(mangaSource.Id, mangaSource.MangaId, mangaSource.SourceId, mangaSource.Url);
    }
}