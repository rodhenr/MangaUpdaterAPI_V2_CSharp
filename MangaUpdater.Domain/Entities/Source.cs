﻿using System.ComponentModel.DataAnnotations;

namespace MangaUpdater.Domain.Entities;

public class Source
{
    public Source(string name, string baseURL)
    {
        Name = name;
        BaseURL = baseURL;
    }

    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(100)]
    public string BaseURL { get; set; }

    public ICollection<UserManga> UserMangas { get; set; }
    public ICollection<MangaSource> MangaSources { get; set; }
    public ICollection<Chapter> Chapters { get; set; }
}