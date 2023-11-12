namespace MangaUpdater.Application.Models.External.MangaDex;

public class MangaDexResponse
{
    public required string Id { get; set; }
    public required string Type { get; set; }
    public MangaDexAttributes Attributes { get; set; }
    public List<MangaDexRelationships> Relationships { get; set; }
}