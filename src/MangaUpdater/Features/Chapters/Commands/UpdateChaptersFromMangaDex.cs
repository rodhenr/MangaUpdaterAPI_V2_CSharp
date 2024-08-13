using System.Globalization;
using MangaUpdater.Database;
using MangaUpdater.DTOs;
using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Extensions;
using MangaUpdater.Features.Chapters.Queries;
using MangaUpdater.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.Chapters.Commands;

public record UpdateChaptersFromMangaDexCommand(int MangaId, int SourceId, string SourceUrl) : IRequest;

public sealed class GetMangasFromMangaDexHandler : IRequestHandler<UpdateChaptersFromMangaDexCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly HttpClient _httpClient;
    private readonly IMediator _mediator;
    private readonly List<Chapter> _chapterList = [];
    private const string ApiOptions = "feed?translatedLanguage[]=en&limit=199&order[chapter]=asc&limit=500&offset=";
    
    public GetMangasFromMangaDexHandler(AppDbContextIdentity context, IHttpClientFactory clientFactory, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
        _httpClient = clientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "MangaUpdater/1.0");
    }

    public async Task Handle(UpdateChaptersFromMangaDexCommand request, CancellationToken cancellationToken)
    {
        var mangaSource = await _context.MangaSources
            .AsNoTracking()
            .GetMangaSourceQueryable(request.MangaId, request.SourceId, cancellationToken);

        if (mangaSource is null)
        {
            throw new EntityNotFoundException($"MangaSource not found for MangaId {request.MangaId} and SourceId {request.SourceId}");
        }
        
        var lastChapterNumber =  await _mediator.Send(new GetLastChapterByNumberQuery(request.MangaId, request.SourceId), cancellationToken);
        var offset = 0;

        while (true)
        {
            var result = await GetApiResult(request, mangaSource.Url, offset, cancellationToken);

            if (result is null || result.Data.Count == 0) break;

            ProcessApiResult(request, result.Data, lastChapterNumber.Number);
            offset += 200;
        }
        
        _context.Chapters.AddRange(_chapterList.Distinct(new ChapterEqualityComparer()).ToList());
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<MangaDexDto?> GetApiResult(UpdateChaptersFromMangaDexCommand request, string mangaUrl, int offset, CancellationToken cancellationToken)
    {
        var url = $"{request.SourceUrl}{mangaUrl}/{ApiOptions}{offset}";
        var response = await _httpClient.GetAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException($"Failed to retrieve data for MangaId `{request.MangaId}` from MangaDex");
        }

        return await response.Content.TryToReadJsonAsync<MangaDexDto>();
    }

    private void ProcessApiResult(UpdateChaptersFromMangaDexCommand request, List<MangaDexResponse> apiData, float lastChapterNumber)
    {
        var response = apiData
            .Select(chapter => new { chapter, ChapterNumber = float.Parse(chapter.Attributes.Chapter, CultureInfo.InvariantCulture) })
            .Where(x => !(x.ChapterNumber <= lastChapterNumber))
            .Select(x => x.chapter);
        
        foreach (var chapter in response)
        {
            _chapterList.Add(new Chapter
            {
                MangaId = request.MangaId,
                SourceId = request.SourceId,
                Number = chapter.Attributes.Chapter,
                Date = DateTime.SpecifyKind(DateTime.Parse(chapter.Attributes.CreatedAt), DateTimeKind.Utc)
            });
        }
    }
}