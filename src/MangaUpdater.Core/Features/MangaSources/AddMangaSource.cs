using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using MangaUpdater.Data.Queries;
using MediatR;

namespace MangaUpdater.Core.Features.MangaSources;

public record AddMangaSourceCommand(int MangaId, SourceInfo SourceInfo) : IRequest;

public record SourceInfo(int SourceId, string MangaUrl);

public sealed class AddMangaSourceHandler : IRequestHandler<AddMangaSourceCommand>
{
    private readonly AppDbContextIdentity _context;
    
    public AddMangaSourceHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task Handle(AddMangaSourceCommand request, CancellationToken cancellationToken)
    {
        _ = await _context.Mangas.GetById(request.MangaId, cancellationToken) ?? throw new EntityNotFoundException($"Manga not found for ID {request.MangaId}.");
        _ = await _context.Sources.GetById(request.SourceInfo.SourceId, cancellationToken) ?? throw new EntityNotFoundException($"Source not found for ID {request.SourceInfo.SourceId}.");
        
        _context.MangaSources.Add(new MangaSource { MangaId = request.MangaId, SourceId = request.SourceInfo.SourceId, Url = request.SourceInfo.MangaUrl });
        await _context.SaveChangesAsync(cancellationToken);
    }
}