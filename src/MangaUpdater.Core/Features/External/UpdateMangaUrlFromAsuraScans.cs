using HtmlAgilityPack;
using MangaUpdater.Core.Features.Chapters;
using MangaUpdater.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.External;

public record UpdateMangaUrlFromAsuraScansCommand : IRequest;

public sealed class SearchForMangaInAsuraScansHandler : IRequestHandler<UpdateMangaUrlFromAsuraScansCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly HttpClient _httpClient;
    private readonly IMediator _mediator;
    
    public SearchForMangaInAsuraScansHandler(AppDbContextIdentity context, IHttpClientFactory clientFactory, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
        _httpClient = clientFactory.CreateClient();
    }
    
    public async Task Handle(UpdateMangaUrlFromAsuraScansCommand request, CancellationToken cancellationToken)
    {
        var mangas = await _context.Mangas
            .AsNoTracking()
            .Where(x => x.MangaSources.Any(y => y.SourceId == (int)SourceEnum.AsuraScans))
            .Select(x => new
            {
                x.MangaTitles, 
                MangaSources = x.MangaSources.Where(y => y.SourceId == (int)SourceEnum.AsuraScans)!.SingleOrDefault()!
            })
            .ToListAsync(cancellationToken);

        foreach (var manga in mangas)
        {
            foreach (var title in manga.MangaTitles)
            {
                var splittedTitle = title.Name.Split(' ');
                var queryString = string.Join("+", splittedTitle);
                
                var html = await _httpClient.GetStringAsync($"https://asuratoon.com/?s={queryString}", cancellationToken);
        
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
        
                var htmlResult = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'bsx')]//a");

                if (htmlResult is null) continue;

                var searchResult = htmlResult
                    .Select(x => new
                    {
                        Url = x.GetAttributeValue("href", ""), 
                        Title = x.GetAttributeValue("Title", "")
                    })
                    .ToList();

                var mangaToUpdate = searchResult.SingleOrDefault(x => x.Title == title.Name && x.Url != manga.MangaSources.Url);
                
                // Update sourceUrl
                await UpdateMangaUrl();
                    
                if (mangaToUpdate is not null) break;
            }
        }
    }

    private async Task UpdateMangaUrl()
    {
        
    }
}