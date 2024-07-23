using MangaUpdater.DTOs;
using MangaUpdater.Entities;
using Riok.Mapperly.Abstractions;

namespace MangaUpdater.Mappers;

[Mapper]
public static partial class ChapterMapper
{
    public static partial IEnumerable<ChapterDto> ToDto(this ICollection<Chapter> chapters);
}