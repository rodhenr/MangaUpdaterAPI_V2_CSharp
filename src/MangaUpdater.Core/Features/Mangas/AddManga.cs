using Microsoft.AspNetCore.Mvc;
using MediatR;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Mangas;

public record AddMangaQuery([FromQuery] int MalId) : IRequest<AddMangaResponse>;
public record AddMangaResponse(Manga Manga);

public sealed class AddMangaHandler : IRequestHandler<AddMangaQuery, AddMangaResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public AddMangaHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<AddMangaResponse> Handle(AddMangaQuery request, CancellationToken cancellationToken)
    {
        var result = await myanimeListService.RegisterMangaFromMyAnimeListById(request.MalId);

        return new AddMangaResponse(result);
    }
}