using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Dtos;

public record MangaDataWithHighlightedMangasDto(MangaModel Data, IEnumerable<MangaUserDto> HighlightedMangas);