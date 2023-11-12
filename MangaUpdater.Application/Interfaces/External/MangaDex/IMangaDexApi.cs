using MangaUpdater.Application.Models.External.MangaDex;

namespace MangaUpdater.Application.Interfaces.External.MangaDex;

public interface IMangaDexApi
{
    Task<List<MangaDexResponse>> GetChaptersAsync(string mangaDexId, float initialChapter = 0);
}