using MangaUpdater.DTOs;
using MangaUpdater.Entities;
using Riok.Mapperly.Abstractions;

namespace MangaUpdater.Mappers;

[Mapper]
public static partial class ChapterMapper
{
    private static partial ChapterDto ToDto(this Chapter chapter);

    public static IEnumerable<ChapterDto> ToDtos(this IEnumerable<Chapter> chapters)
    {
        return chapters.Select(ToDto);
    }
}