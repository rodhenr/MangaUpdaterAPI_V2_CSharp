using MangaUpdater.Core.Models;
using MangaUpdater.Data.Entities.Models;
using Riok.Mapperly.Abstractions;

namespace MangaUpdater.Application.Mappings;

[Mapper]
public static partial class MangaMapper
{
    public static partial MangaModel ToModel(Manga manga);
    
    [MapTo(nameof(MangaModel.Genres))]
    public IEnumerable<string> GetGenres(IEnumerable<MangaGenre> mangaGenres)
    {
        // Implement logic to get the genres
        return new List<string>(); // Placeholder
    }
}