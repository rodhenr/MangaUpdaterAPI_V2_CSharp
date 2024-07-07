using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.Controllers.Shared;

[ApiController]
[Route("api/[controller]")]
[EnableCors]
public class BaseController : ControllerBase;