﻿using System.ComponentModel.DataAnnotations;

namespace MangaUpdater.Domain.Entities;

public class MangaRegister
{
    public MangaRegister(int id, string coverURL, string name, string alternativeName, string author, string synopsis, string type, string myAnimeListURL, int sourceId, string genres)
    {
        Id = id;
        CoverURL = coverURL;
        Name = name;
        AlternativeName = alternativeName;
        Author = author;
        Synopsis = synopsis;
        Type = type;
        MyAnimeListURL = myAnimeListURL;
        SourceId = sourceId;
        Genres = genres;
    }

    public int Id { get; set; }

    [MaxLength(200)]
    public string CoverURL { get; set; }

    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(200)]
    public string AlternativeName { get; set; }

    [MaxLength(50)]
    public string Author { get; set; }

    [MaxLength(2000)]
    public string Synopsis { get; set; }

    [MaxLength(20)]
    public string Type { get; set; }

    [MaxLength(200)]
    public string MyAnimeListURL { get; set; }

    public int SourceId { get; set; }

    [MaxLength(50)]
    public string Genres { get; set; }
}
