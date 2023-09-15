﻿using System.Text.Json.Serialization;

namespace MangaUpdater.Application.Models;

public class MangaLivreReleaseInfo
{
    [JsonPropertyName("scanlators")] public required List<MangaLivreScanlator> ScanlatorInfo { get; set; }
    [JsonPropertyName("link")] public required string Link {get; set;}
}