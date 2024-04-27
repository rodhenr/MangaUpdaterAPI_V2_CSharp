using HtmlAgilityPack;
using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Features.Chapters;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
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
            .Where(x => x.MangaSources.Any(y => y.SourceId == (int)SourceEnum.AsuraScans) && x.MangaTitles.Any(y => y.IsAsuraMainTitle))
            .Select(x => new
            {
                Title = x.MangaTitles.First(y => y.IsAsuraMainTitle == true), 
                Source = x.MangaSources.First(y => y.SourceId == (int)SourceEnum.AsuraScans)
            })
            .ToListAsync(cancellationToken);

        var asuraScansSource = await _context.Sources
            .Where(x => x.Id == (int)SourceEnum.AsuraScans)
            .SingleOrDefaultAsync(cancellationToken) ?? throw new EntityNotFoundException("Source not found");

        foreach (var manga in mangas)
        {
            await SearchAndUpdateSourceUrl(asuraScansSource.BaseUrl, manga.Title, manga.Source, cancellationToken);
        }
    }

    private async Task SearchAndUpdateSourceUrl(string baseUrl, MangaTitle title, MangaSource source, CancellationToken cancellationToken)
    {
        var splittedTitle = title.Name.Split(' ');
        var queryString = string.Join("+", splittedTitle);
            
        var html = await _httpClient.GetStringAsync($"{baseUrl.Replace("manga/", "")}?s={queryString}", cancellationToken);
    
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);
    
        var htmlResult = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'bsx')]//a");
        var newUrl = htmlResult?
            .Where(x => x.GetAttributeValue("Title", "").StartsWith(title.Name))
            .Select(x => x.GetAttributeValue("href", ""))
            .SingleOrDefault(); 

        if (newUrl is null) return;
        
        await UpdateMangaUrl(baseUrl, source, newUrl, cancellationToken);
    }

    private async Task UpdateMangaUrl(string baseUrl, MangaSource mangaSource, string url, CancellationToken cancellationToken)
    {
        mangaSource.Url = url.Replace($"{baseUrl}", "");
        await _context.SaveChangesAsync(cancellationToken);
    }
}