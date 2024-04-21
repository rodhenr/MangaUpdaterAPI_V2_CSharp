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
        await ValidateMangaAndSource(request, cancellationToken);
        
        _context.MangaSources.Add(new MangaSource { MangaId = request.MangaId, SourceId = request.SourceInfo.SourceId, Url = request.SourceInfo.MangaUrl });
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidateMangaAndSource(AddMangaSourceCommand request, CancellationToken cancellationToken)
    {
        var manga = await _context.Mangas.GetById(request.MangaId, cancellationToken);

        if (manga is null) throw new EntityNotFoundException($"Manga not found for ID {request.MangaId}.");
        
        var source = await _context.Sources.GetById(request.SourceInfo.SourceId, cancellationToken);
        
        if (source is null) throw new EntityNotFoundException($"Source not found for ID {request.SourceInfo.SourceId}.");
    }
}