using MangaUpdater.DTOs;
using MangaUpdater.Entities;
using Riok.Mapperly.Abstractions;

namespace MangaUpdater.Mappers;

[Mapper]
public static partial class MangaAuthorMapper
{
    private static partial MangaAuthorDto ToDto(this MangaAuthor mangaAuthor);

    public static IEnumerable<MangaAuthorDto> ToDtos(this IEnumerable<MangaAuthor> mangaAuthors)
    {
        return mangaAuthors.Select(ToDto);
    }
}