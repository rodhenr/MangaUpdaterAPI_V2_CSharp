﻿using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class Chapter : Entity
{
    public required int MangaId { get; set; }
    public required int SourceId { get; set; }
    public required DateTime Date { get; set; }
    public required float Number { get; set; }

    [JsonIgnore] public Manga? Manga { get; set; }

    [JsonIgnore] public Source? Source { get; set; }
}