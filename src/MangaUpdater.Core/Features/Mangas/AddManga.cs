using MangaUpdater.Core.Features.External;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Mangas;

public record AddMangaQuery([FromQuery] int MalId) : IRequest<AddMangaResponse>;
public record AddMangaResponse(Manga Manga);

public sealed class AddMangaHandler : IRequestHandler<AddMangaQuery, AddMangaResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly ISender _mediator;
    
    public AddMangaHandler(AppDbContextIdentity context, ISender mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<AddMangaResponse> Handle(AddMangaQuery request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateMangaFromMyAnimeListQuery(request.MalId), cancellationToken);

        var result = await _context.Mangas.FirstOrDefaultAsync(x => x.MyAnimeListId == request.MalId, cancellationToken);

        if (result is null) throw new Exception("Error...");
        
        return new AddMangaResponse(result);
    }
}