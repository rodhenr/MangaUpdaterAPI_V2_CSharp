using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Features.External;
using MangaUpdater.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Chapters;

public enum SourceEnum {
    MangaDex = 1,
    AsuraScans = 2
}

public record UpdateChaptersCommand([FromRoute] int MangaId, [FromRoute] SourceEnum SourceId) : IRequest;

public sealed class UpdateChaptersHandler : IRequestHandler<UpdateChaptersCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly ISender _mediator;
    
    public UpdateChaptersHandler(AppDbContextIdentity context, ISender mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(UpdateChaptersCommand request, CancellationToken cancellationToken)
    {
        var source = await _context.Sources
            .Where(x => x.Id == (int)request.SourceId)
            .FirstOrDefaultAsync(cancellationToken);

        if (source is null) throw new EntityNotFoundException($"Source not found for ID {request.SourceId}");
        
        switch (request.SourceId)
        {
            case SourceEnum.MangaDex: 
                await _mediator.Send(new UpdateChaptersFromMangaDexCommand(request.MangaId, source.Id, source.BaseUrl), cancellationToken);
                break;
            case SourceEnum.AsuraScans: 
                await _mediator.Send(new UpdateChaptersFromAsuraScansCommand(request.MangaId, source.Id, source.BaseUrl), cancellationToken);
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }
}