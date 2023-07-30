using MangaUpdater.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChapterController : ControllerBase
{
    private readonly IUserMangaService _userMangaService;
    private readonly IChapterService _chapterService;

    public ChapterController(IUserMangaService userMangaService, IChapterService chapterService)
    {
        _userMangaService = userMangaService;
        _chapterService = chapterService;
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateUserMangaStatus(int userId, int mangaId, int sourceId, int chapterId)
    {
        try
        {
            var chapter = await _chapterService.GetChapterById(chapterId);

            if(chapter == null)
            {
                return BadRequest("This chapter doesn't exist");
            }

            if(chapter.SourceId != sourceId || chapter.MangaId != mangaId)
            {
               return BadRequest("This chapter doesn't belongs to this manga or source");
            }

            await _userMangaService.UpdateUserMangaAsync(userId, mangaId, sourceId, chapterId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
