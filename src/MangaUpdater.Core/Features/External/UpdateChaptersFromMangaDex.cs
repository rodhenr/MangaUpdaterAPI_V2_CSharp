using System.Globalization;
using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Common.Extensions;
using MangaUpdater.Core.Common.Helpers;
using MangaUpdater.Core.Features.Chapters;
using MangaUpdater.Core.Features.MangaSources;
using MangaUpdater.Core.Models;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using MediatR;

namespace MangaUpdater.Core.Features.External;

public record UpdateChaptersFromMangaDexCommand(int MangaId, int SourceId, string SourceUrl) : IRequest;

public sealed class GetMangasFromMangaDexHandler : IRequestHandler<UpdateChaptersFromMangaDexCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly HttpClient _httpClient;
    private readonly IMediator _mediator;
    private readonly List<Chapter> _chapterList = [];
    
    public GetMangasFromMangaDexHandler(AppDbContextIdentity context, IHttpClientFactory clientFactory, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
        _httpClient = clientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "MangaUpdater/1.0");
    }

    public async Task Handle(UpdateChaptersFromMangaDexCommand request, CancellationToken cancellationToken)
    {
        var mangaSource = await _mediator.Send(new GetMangaSourceQuery(request.MangaId, request.SourceId), cancellationToken);
        var lastChapterNumber =  await _mediator.Send(new GetLastChapterNumberQuery(request.MangaId, request.SourceId), cancellationToken);
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

    private async Task<MangaDexModel?> GetApiResult(UpdateChaptersFromMangaDexCommand request, string mangaUrl, int offset, CancellationToken cancellationToken)
    {
        var options = $"feed?translatedLanguage[]=en&limit=199&order[chapter]=asc&limit=500&offset={offset}";
        var url = $"{request.SourceUrl}{mangaUrl}/{options}";

        var response = await _httpClient.GetAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode) throw new BadRequestException($"Failed to retrieve data for MangaId `{request.MangaId}` from MangaDex");

        return await response.Content.TryToReadJsonAsync<MangaDexModel>();
    }

    private void ProcessApiResult(UpdateChaptersFromMangaDexCommand request, List<MangaDexResponse> apiData, float lastChapterNumber)
    {
        foreach (var chapter in from chapter in apiData let chapterNumber = float.Parse(chapter.Attributes.Chapter, CultureInfo.InvariantCulture) where !(chapterNumber <= lastChapterNumber) select chapter)
        {
            _chapterList.Add(new Chapter
            {
                MangaId = request.MangaId,
                SourceId = request.SourceId,
                Number = chapter.Attributes.Chapter,
                Date = DateTime.Parse(chapter.Attributes.CreatedAt)
            });
        }
    }
}