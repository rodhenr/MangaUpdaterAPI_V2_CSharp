using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Entities.Models;

public sealed class Genre
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [StringLength(20)]
    [Unicode(false)]
    public required string Name { get; set; }

    [InverseProperty("Genre")]
    [JsonIgnore] 
    public IEnumerable<MangaGenre>? MangaGenres { get; set; }
}