using MangaUpdater.DTOs;
using MangaUpdater.Entities;
using Riok.Mapperly.Abstractions;

namespace MangaUpdater.Mappers;

[Mapper]
public static partial class MangaGenreMapper
{
   //  public static IEnumerable<GenreDto> ToDtos(this IEnumerable<MangaGenre> mangaGenres)
   //  {
   //      return mangaGenres.Select(ToDto);
   //  }
   //  
   //  [MapProperty(nameof(MangaGenre.GenreId), nameof(GenreDto.Id))]
   //  [MapProperty(nameof(MangaGenre.Genre.Name), nameof(GenreDto.Name))]
   // private static partial GenreDto ToDto(this MangaGenre mangaGenre);
   [MapProperty(nameof(MangaGenre.Genre) + "." + nameof(Genre.Id), nameof(GenreDto.Id))]
   [MapProperty(nameof(MangaGenre.Genre) + "." + nameof(Genre.Name), nameof(GenreDto.Name))]
   private static partial GenreDto ToDto(this MangaGenre mangaGenre);

   public static IEnumerable<GenreDto> ToDtos(this IEnumerable<MangaGenre> mangaGenres)
   {
      return mangaGenres.Select(ToDto);
   }
}