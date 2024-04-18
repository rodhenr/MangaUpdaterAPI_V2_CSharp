using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.Core.Features.MangaSources;

public record AddMangaSourceQuery([FromQuery] int MangaId, [FromBody] int SourceId, [FromBody] string MangaUrl) : IRequest<AddMangaSourceResponse>;
public record AddMangaSourceResponse;

public sealed class AddMangaSourceHandler : IRequestHandler<AddMangaSourceQuery, AddMangaSourceResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public AddMangaSourceHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<AddMangaSourceResponse> Handle(AddMangaSourceQuery request, CancellationToken cancellationToken)
    {
        // TODO: Add validation
        _context.MangaSources.Add(new MangaSource { MangaId = request.MangaId, SourceId = request.SourceId, Url = request.MangaUrl });
        await _context.SaveChangesAsync(cancellationToken);

        return new AddMangaSourceResponse();
    }
}