﻿using System.ComponentModel.DataAnnotations;

namespace MangaUpdater.Domain.Entities;

public class MangaSource
{
    public MangaSource(int mangaId, int sourceId, string uRL)
    {
        MangaId = mangaId;
        SourceId = sourceId;
        URL = uRL;
    }

    public int MangaId { get; set; }

    public int SourceId { get; set; }

    [MaxLength(100)]
    public string URL { get; set; }

    public Manga Manga { get; set; }
    public Source Source { get; set; }
}