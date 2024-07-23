using MangaUpdater.DTOs;
using MangaUpdater.Entities;
using Riok.Mapperly.Abstractions;

namespace MangaUpdater.Mappers;

[Mapper]
public static partial class MangaSourceMapper
{
    [MapProperty([nameof(MangaSource.Source), nameof(MangaSource.Source.Id)], [nameof(SourceDto.Id)])]
    [MapProperty([nameof(MangaSource.Source), nameof(Source.Name)], [nameof(SourceDto.Name)])]
    [MapPropertyFromSource(nameof(SourceDto.Url), Use = nameof(MapFullUrl))]
    private static partial SourceDto ToDto(this MangaSource mangaSource);
    
    [UserMapping(Default = false)]
    private static string MapFullUrl(MangaSource mangaSource) => $"{mangaSource.Url}{mangaSource.Source.BaseUrl}";
    
    public static IEnumerable<SourceDto> ToDtos(this IEnumerable<MangaSource> mangaSources)
    {
        return mangaSources.Select(ToDto);
    }
}