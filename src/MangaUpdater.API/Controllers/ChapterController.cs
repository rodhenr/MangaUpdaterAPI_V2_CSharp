using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Core.Features.Chapters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.API.Controllers;

public class ChapterController(ISender mediator) : BaseController
{
    [HttpGet("/{mangaId:int}/chapter/{chapterId:int}")]
    public async Task<GetChapterResponse> GetChaptersByIdAndMangaId([FromQuery] GetChapterQuery request)
    {
        return await mediator.Send(request);
    }
    
    /// <summary>
    /// Update chapters from a combination of manga and source.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    /// <response code="403">Unauthorized</response>
    [Authorize(Policy = "Admin")]
    [SwaggerOperation("Update chapters from a combination of manga and source.")]
    [HttpPost("/{mangaId:int}/source/{sourceId:int}/chapters")]
    public async Task<ActionResult> UpdateChaptersFromSource(int mangaId, int sourceId)
    {
        var mangaSource = await _mangaSourceService.GetByMangaIdAndSourceId(mangaId, sourceId);
        var source = await _sourceService.GetById(sourceId);
        var lastChapter = await _chapterService.GetLastByMangaIdAndSourceId(mangaId, sourceId);

        await _externalSourceService.UpdateChapters(new MangaInfoToUpdateChapters(mangaId, sourceId, mangaSource.Url,
            source.BaseUrl, source.Name, lastChapter?.Number));

        return Ok();
    }
}