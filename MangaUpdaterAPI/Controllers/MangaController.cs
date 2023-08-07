using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MangaUpdater.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MangaController : ControllerBase
{
    private readonly IMangaService _mangaService;
    private readonly IUserMangaChapterService _userMangaChapterService;
    private readonly IUserSourceService _userSourceService;
    private readonly IChapterService _chapterService;
    private readonly IUserMangaService _userMangaService;

    public MangaController(IMangaService mangaService, IUserMangaChapterService userMangaChapterService, IUserSourceService userSourceService, IChapterService chapterService, IUserMangaService userMangaService)
    {
        _mangaService = mangaService;
        _userMangaChapterService = userMangaChapterService;
        _userSourceService = userSourceService;
        _chapterService = chapterService;
        _userMangaService = userMangaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MangaUserDTO>>> GetMangas([FromQuery] string? orderBy = null, [FromQuery] List<int>? sourceId = null, [FromQuery] List<int>? genreId = null)
    {

        var mangas = await _mangaService.GetMangasWithFilter(orderBy, sourceId, genreId);

        return Ok(mangas);
    }

    [HttpGet("{mangaId}")]
    public async Task<ActionResult<MangaDTO>> GetManga(int mangaId, int userId)
    {
        var manga = await _mangaService.GetMangaByIdAndUserId(mangaId, userId);

        if (manga == null)
        {
            return BadRequest($"Manga not found for id {mangaId}");
        }

        return Ok(manga);
    }

    [HttpPost("{mangaId}/follow")]
    public async Task<ActionResult> FollowManga(int mangaId, int userId, IEnumerable<int> sourceIdList)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest($"Manga not found for id {mangaId}");
        }

        var userSources = await _userSourceService.GetUserSourcesByMangaId(mangaId, userId);

        await _userMangaChapterService.AddUserMangaBySourceIdList(mangaId, userId, sourceIdList, userSources);

        return Ok();
    }

    [HttpDelete("{mangaId}/unfollow")]
    public async Task<ActionResult> UnfollowManga(int mangaId, int userId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest($"Manga not found for id {mangaId}");
        }

        await _userMangaChapterService.DeleteUserMangasByMangaId(mangaId, userId);

        return Ok();
    }

    [HttpGet("user/me")]
    public async Task<ActionResult<IEnumerable<MangaUserLoggedDTO>>> GetLoggedUserMangas(int userId)
    {
        IEnumerable<MangaUserLoggedDTO> mangas = await _mangaService.GetMangasByUserIdLogged(userId);

        return Ok(mangas);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<MangaUserDTO>>> GetUserMangas(int userId)
    {
        IEnumerable<MangaUserDTO> userMangas = await _mangaService.GetMangasByUserId(userId);

        return Ok(userMangas);
    }

    [HttpGet("user/me/list")]
    public async Task<ActionResult<IEnumerable<MangaUserDTO>>> GetUserMangasList(int userId)
    {
        //Needs to implement JWT token to check the userId
        IEnumerable<MangaUserDTO> userMangas = await _mangaService.GetMangasByUserId(userId);

        return Ok(userMangas);
    }

    [HttpGet("{mangaId}/sources")]
    public async Task<ActionResult<IEnumerable<UserSourceDTO>>> GetUserSources(int mangaId, int userId)
    {
        var userSources = await _userSourceService.GetUserSourcesByMangaId(mangaId, userId);

        if (userSources == null)
        {
            return BadRequest($"No sources found for mangaId {mangaId}");
        }

        return Ok(userSources);
    }

    [HttpPost("{mangaId}/source/follow")]
    public async Task<ActionResult> AddUserManga(int mangaId, int userId, int sourceId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest("Manga not found");
        }

        await _userMangaChapterService.AddUserManga(mangaId, userId, sourceId);

        return Ok();
    }

    [HttpDelete("{mangaId}/source/unfollow")]
    public async Task<ActionResult> DeleteUserManga(int mangaId, int userId, int sourceId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest("Manga not found");
        }

        await _userMangaChapterService.DeleteUserManga(mangaId, userId, sourceId);

        return Ok();
    }

    [HttpPatch("{mangaId}/source/{sourceId}/chapter")]
    public async Task<ActionResult> UpdateManga(int mangaId, int sourceId, int userId, int chapterId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest("Manga not found");
        }

        var chapter = await _chapterService.GetChapterById(chapterId);

        if (chapter == null)
        {
            return BadRequest("Chapter not found");
        }

        await _userMangaService.UpdateUserMangaAsync(userId, mangaId, sourceId, chapterId);

        return Ok();
    }
}
