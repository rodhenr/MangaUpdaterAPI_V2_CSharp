﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaUpdater.Entities;

public partial class Manga
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int MyAnimeListId { get; set; }
    
    [StringLength(200)]
    public string CoverUrl { get; set; } = null!;

    [StringLength(2000)]
    public string Synopsis { get; set; } = null!;

    [StringLength(50)]
    public string Type { get; set; } = null!;

    [InverseProperty("Manga")]
    public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

    [InverseProperty("Manga")]
    public virtual ICollection<MangaAuthor> MangaAuthors { get; set; } = new List<MangaAuthor>();

    [InverseProperty("Manga")]
    public virtual ICollection<MangaGenre> MangaGenres { get; set; } = new List<MangaGenre>();

    [InverseProperty("Manga")]
    public virtual ICollection<MangaSource> MangaSources { get; set; } = new List<MangaSource>();

    [InverseProperty("Manga")]
    public virtual ICollection<MangaTitle> MangaTitles { get; set; } = new List<MangaTitle>();

    [InverseProperty("Manga")]
    public virtual ICollection<UserManga> UserMangas { get; set; } = new List<UserManga>();
}
