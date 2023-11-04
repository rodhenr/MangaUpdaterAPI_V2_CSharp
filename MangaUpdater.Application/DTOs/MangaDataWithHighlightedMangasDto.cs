using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.DTOs;

public record MangaDataWithHighlightedMangasDto(MangaDto Data, IEnumerable<MangaUserDto> HighlightedMangas);