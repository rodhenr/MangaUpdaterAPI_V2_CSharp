using MangaUpdater.Entities;
using Riok.Mapperly.Abstractions;
using MangaUpdater.Features.Mangas.Queries;

namespace MangaUpdater.Mappers;

[Mapper]
public static partial class MangaMapper
{
    [MapProperty(nameof(Manga.MyAnimeListId), nameof(MangaInfo.MangaId))]
    [MapPropertyFromSource(nameof(MangaInfo.MangaName), Use = nameof(MapFirstMangaTitle))]
    private static partial MangaInfo ToDto(this Manga manga);
    
    [UserMapping(Default = false)]
    private static string MapFirstMangaTitle(Manga manga) 
        => manga.MangaTitles.First().Name;

    public static IEnumerable<MangaInfo> ToDtos(this IEnumerable<Manga> mangas)
    {
        return mangas.Select(ToDto);
    }
}