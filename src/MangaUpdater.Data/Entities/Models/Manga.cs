using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Entities.Models;

public class Manga
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [StringLength(200)]
    [Unicode(false)]
    public required string CoverUrl { get; set; }
    
    [StringLength(2000)]
    [Unicode(false)]
    public required string Synopsis { get; set; }
    
    [StringLength(50)]
    [Unicode(false)]
    public required string Type { get; set; }
    
    [DefaultValue(0)]
    public required int MyAnimeListId { get; set; }

    [InverseProperty("Manga")]
    [JsonIgnore]
    public List<Chapter> Chapters { get; set; } = [];
    
    [InverseProperty("Manga")]
    [JsonIgnore] 
    public List<UserManga> UserMangas { get; set; } = [];
    
    [InverseProperty("Manga")]
    [JsonIgnore] 
    public List<MangaAuthor> MangaAuthors { get; set; } = [];
    
    [InverseProperty("Manga")]
    [JsonIgnore] 
    public List<MangaGenre> MangaGenres { get; set; } = [];
    
    [InverseProperty("Manga")]
    [JsonIgnore] 
    public List<MangaSource> MangaSources { get; set; } = [];
    
    [InverseProperty("Manga")]
    [JsonIgnore] 
    public List<MangaTitle> MangaTitles { get; set; } = [];
}