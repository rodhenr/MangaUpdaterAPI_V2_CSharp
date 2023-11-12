using MangaUpdater.Application.Models.External.MangaDex;

namespace MangaUpdater.Infra.Data.ExternalServices.MangaDex;

public class MangaDexApiData
{
    public required string Result { get; set; }
    public required string Response { get; set; }
    public List<MangaDexResponse> Data { get; set; }
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public int? Total { get; set; }
}