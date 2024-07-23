using MangaUpdater.DTOs;
using MangaUpdater.Entities;
using Riok.Mapperly.Abstractions;

namespace MangaUpdater.Mappers;

[Mapper]
public static partial class MangaSourceMapper
{
    public static IEnumerable<SourceDto> ToDtos(this IEnumerable<MangaSource> mangaSources)
    {
        return mangaSources.Select(ToDto);
    }
    
    [MapProperty(nameof(MangaSource.Source) + "." + nameof(Source.Id), nameof(SourceDto.Id))]
    [MapProperty(nameof(MangaSource.Source) + "." + nameof(Source.Name), nameof(SourceDto.Name))]
    [MapProperty(nameof(MangaSource.Source) + "." + nameof(Source.BaseUrl), nameof(SourceDto.Url))]
    private static partial SourceDto ToDto(this MangaSource mangaSource);
}