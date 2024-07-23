using MangaUpdater.DTOs;
using MangaUpdater.Entities;
using Riok.Mapperly.Abstractions;

namespace MangaUpdater.Mappers;

[Mapper]
public static partial class MangaAuthorMapper
{
    public static partial IEnumerable<MangaAuthorDto> ToDto(this ICollection<MangaAuthor> chapter);
}