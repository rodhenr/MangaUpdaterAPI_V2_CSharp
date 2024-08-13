using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using MangaUpdater.Database;
using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Extensions;
using MangaUpdater.Features.Chapters.Queries;
using MangaUpdater.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using InvalidOperationException = MangaUpdater.Exceptions.InvalidOperationException;

namespace MangaUpdater.Features.Chapters.Commands;

public record UpdateChaptersFromAsuraScansCommand(int MangaId, int SourceId, string SourceUrl) : IRequest;

public sealed partial class GetMangasFromAsuraScansHandler : IRequestHandler<UpdateChaptersFromAsuraScansCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly HttpClient _httpClient;
    private readonly IMediator _mediator;
    private readonly List<Chapter> _chapterList = [];

    public GetMangasFromAsuraScansHandler(AppDbContextIdentity context, IHttpClientFactory clientFactory,
        IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
        _httpClient = clientFactory.CreateClient();
    }

    public async Task Handle(UpdateChaptersFromAsuraScansCommand request, CancellationToken cancellationToken)
    {
        var mangaSource = await _context.MangaSources
            .AsNoTracking()
            .GetMangaSourceQueryable(request.MangaId, request.SourceId, cancellationToken);

        if (mangaSource is null)
        {
            throw new EntityNotFoundException($"MangaSource not found for MangaId {request.MangaId} and SourceId {request.SourceId}");
        }
        
        var lastChapterNumber = await _mediator.Send(new GetLastChapterByNumberQuery(request.MangaId, request.SourceId), cancellationToken);

        var html = await _httpClient.GetStringAsync($"{request.SourceUrl}{mangaSource.Url}", cancellationToken);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);
            
        var chapterNodes = htmlDoc.DocumentNode
            .SelectNodes("//div/h3/a[contains(., 'Chapter')]/ancestor::div[1]")
            .Descendants("h3")
            .Where(h3 => h3.Descendants("a").Any(a => a.InnerText.Contains("Chapter")));
        
        ProcessApiResult(request, chapterNodes, lastChapterNumber.Number);

        _context.Chapters.AddRange(_chapterList.Distinct(new ChapterEqualityComparer()).ToList());
        await _context.SaveChangesAsync(cancellationToken);
    }

    private void ProcessApiResult(UpdateChaptersFromAsuraScansCommand request, IEnumerable<HtmlNode> nodes, float lastChapterNumber)
    {
        foreach (var chapterNode in nodes)
        {
            var chapterNumberString = chapterNode.Descendants("a")
                .First()
                .GetAttributeValue("href", "")
                .Split('/')
                .LastOrDefault()?
                .Trim() ?? throw new InvalidOperationException("Chapter number is invalid.");
            
            var chapterNumber = ExtractNumberFromString(chapterNumberString);
            if (chapterNumber <= lastChapterNumber) break;

            var chapterDateString = chapterNode.NextSibling?.InnerText.Trim() 
                                    ?? throw new InvalidOperationException("Chapter date is invalid.");

            var parsedDate = ParseDate(chapterDateString);
            var chapterDate = new DateTime(parsedDate.Year, parsedDate.Month, parsedDate.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

            var chapter = new Chapter
            {
                MangaId = request.MangaId,
                SourceId = request.SourceId,
                Number = chapterNumber.ToString(CultureInfo.InvariantCulture),
                Date = DateTime.SpecifyKind(chapterDate, DateTimeKind.Utc)
            };

            _chapterList.Add(chapter);
        }
    }
    
    private static DateTime ParseDate(string dateString)
    {
        const string format = "MMMM d yyyy";
        var cleanedDateString = DateParseRegex().Replace(dateString, "$1");

        if (DateTime.TryParseExact(cleanedDateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
            return parsedDate;
        }
        
        throw new FormatException("Invalid date format.");
    }

    private static float ExtractNumberFromString(string input)
    {
        var match = MyRegex().Match(input);

        if (!match.Success) return 0;

        var numericPart = match.Groups[1].Value;

        if (float.TryParse(numericPart, NumberStyles.Float, CultureInfo.InvariantCulture, out var floatResult))
            return floatResult;
        if (int.TryParse(numericPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intResult))
            return intResult;

        throw new InvalidOperationException("Failed to parse the numeric part as either float or int.");
    }

    [GeneratedRegex(@"(\d+(\.\d+)?)")]
    private static partial Regex MyRegex();
    
    [GeneratedRegex(@"(\d+)(st|nd|rd|th)")]
    private static partial Regex DateParseRegex();
}