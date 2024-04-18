using MangaUpdater.Core.Features.External;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MangaUpdater.Data;

namespace MangaUpdater.Core.Features.Chapters;

public enum SourceEnum {
    MangaDex,
    AsuraScans
}

public record UpdateChaptersQuery([FromQuery] int MangaId, SourceEnum SourceId) : IRequest<UpdateChaptersResponse>;
public record UpdateChaptersResponse();

public sealed class UpdateChaptersHandler : IRequestHandler<UpdateChaptersQuery, UpdateChaptersResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly ISender _mediator;
    
    public UpdateChaptersHandler(AppDbContextIdentity context, ISender mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<UpdateChaptersResponse> Handle(UpdateChaptersQuery request, CancellationToken cancellationToken)
    {
        switch (request.SourceId)
        {
            case SourceEnum.MangaDex: 
                await _mediator.Send(new UpdateChaptersFromMangaDexQuery(request.MangaId), cancellationToken);
                break;
            case SourceEnum.AsuraScans: 
                await _mediator.Send(new UpdateChaptersFromAsuraScansQuery(request.MangaId), cancellationToken);
                break;
            default: break;
        }

        return new UpdateChaptersResponse();
    }
}