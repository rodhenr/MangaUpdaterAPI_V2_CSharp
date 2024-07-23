using MangaUpdater.DTOs;
using MangaUpdater.Entities;
using Riok.Mapperly.Abstractions;

namespace MangaUpdater.Mappers;

[Mapper]
public static partial class MangaTitleMapper
{
    [MapProperty(nameof(MangaTitle.IsMyAnimeListMainTitle), nameof(MangaTitleDto.IsMainTitle))]
    private static partial MangaTitleDto ToDto(this MangaTitle mangaTitle);

    public static IEnumerable<MangaTitleDto> ToDtos(this IEnumerable<MangaTitle> mangaTitles)
    {
        return mangaTitles.Select(ToDto);
    }
    //public static partial IEnumerable<MangaTitleDto> ToDto(this ICollection<MangaTitle> mangaTitles);
}