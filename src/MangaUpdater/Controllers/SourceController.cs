using MangaUpdater.Controllers.Shared;
using MangaUpdater.Features.Sources.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MangaUpdater.Controllers;

public class SourceController(ISender mediator) : BaseController
{
    [AllowAnonymous]
    [SwaggerOperation("Get all sources")]
    [HttpGet]
    public async Task<List<GetSourcesResponse>> GetSources()    
    {
        return await mediator.Send(new GetSourcesQuery());
    }

    [AllowAnonymous]
    [SwaggerOperation("Get a source by id")]
    [HttpGet("{sourceId:int}")]
    public async Task<GetSourceResponse> GetSourceById([FromQuery] GetSourceQuery request)
    {
        return await mediator.Send(request);
    }
}